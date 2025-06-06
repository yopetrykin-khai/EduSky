﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduSky
{
    public class Course
    {
        public string Title { get; set; }
        public string ImagePath { get; set; }
        public List<Task> Tasks { get; set; } = new();
        public List<string> EnrolledUserLogins { get; set; } = new();
        public string TeacherLogin { get; set; }

        public event Action<string>? UserEnrolled;
        public event Action<string>? UserLeft;

        public void EnrollUser(string loginl)
        {
            if (!EnrolledUserLogins.Contains(loginl))
            {
                EnrolledUserLogins.Add(loginl);
                UserEnrolled?.Invoke(loginl);
            }
        }

        public void LeaveCourse(string login)
        {
            if (EnrolledUserLogins.Remove(login))
            {
                foreach (var task in Tasks)
                {
                    task.Answers.RemoveAll(answer => answer.StudentLogin == login);
                }
                UserLeft?.Invoke(login);
            }
        }
    }
}
