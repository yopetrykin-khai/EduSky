using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduSky
{
    public class Teacher : User
    {
        public List<string> CreatedCourseTitles { get; set; } = new();
        public override string GetRole() => "Teacher";

        public bool CreateCourse(string title, string description)
        {
            if (EduSkyProgramm.Courses.Any(c => c.Title == title)) return false;

            var course = new Course
            {
                Title = title,
                Description = description,
                TeacherLogin = this.Login
            };

            EduSkyProgramm.Courses.Add(course); 
            CreatedCourseTitles.Add(title);
            return true;
        }

        public bool AddTaskToCourse(string courseTitle, string taskTitle, string taskDescription, string filePath)
        {
            var course = EduSkyProgramm.Courses.FirstOrDefault(c => c.Title == courseTitle && c.TeacherLogin == this.Login);
            if (course == null) return false;

            var task = new Task
            {
                Title = taskTitle,
                Description = taskDescription,
                MethodicalFilePath = filePath
            };

            course.Tasks.Add(task);
            return true;
        }
    }
}
