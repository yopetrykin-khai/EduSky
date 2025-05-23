using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduSky
{
    public class RegisteredUser : User
    {
        public List<string> EnrolledCourseTitles { get; set; } = new();
        public override string GetRole() => "Student";

        public List<Task> ViewTasks(string courseTitle)
        {
            var course = EduSkyProgramm.Courses.FirstOrDefault(c => c.Title == courseTitle);
            if (course == null || !EnrolledCourseTitles.Contains(courseTitle))
                return new List<Task>();

            return course.Tasks;
        }

        public bool SubmitAnswer(string courseTitle, string taskTitle, string filePath)
        {
            var course = EduSkyProgramm.Courses.FirstOrDefault(c => c.Title == courseTitle);
            if (course == null || !EnrolledCourseTitles.Contains(courseTitle))
                return false;

            var task = course.Tasks.FirstOrDefault(t => t.Title == taskTitle);
            if (task == null) return false;

            task.SubmitAnswer(this.Login, filePath);
            return true;
        }

        public bool EnrollToCourse(string courseTitle)
        {
            var course = EduSkyProgramm.Courses.FirstOrDefault(c => c.Title == courseTitle);
            if (course == null || EnrolledCourseTitles.Contains(courseTitle)) return false;

            EnrolledCourseTitles.Add(courseTitle);
            course.EnrollUser(this.Login);
            return true;
        }

        public bool LeaveCourse(string courseTitle)
        {
            var course = EduSkyProgramm.Courses.FirstOrDefault(c => c.Title == courseTitle);
            if (course == null || !EnrolledCourseTitles.Contains(courseTitle)) return false;

            EnrolledCourseTitles.Remove(courseTitle);
            course.LeaveCourse(this.Login);
            return true;
        }
    }
}
