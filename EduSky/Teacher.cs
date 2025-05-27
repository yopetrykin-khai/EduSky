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

        public bool CreateCourse(string title, string imagepath)
        {
            if (EduSkyProgramm.Courses.Any(c => c.Title == title)) return false;

            var course = new Course
            {
                Title = title,
                ImagePath = imagepath,
                TeacherLogin = this.Login
            };

            EduSkyProgramm.Courses.Add(course);
            EduSkyProgramm.SaveAll();
            CreatedCourseTitles.Add(title);
            return true;
        }
        public List<StudentAnswer>? ViewAnswers(string courseTitle, string taskTitle)
        {
            var course = EduSkyProgramm.Courses.FirstOrDefault(c => c.Title == courseTitle && c.TeacherLogin == this.Login);
            if (course == null) return null;

            var task = course.Tasks.FirstOrDefault(t => t.Title == taskTitle);
            return task?.Answers;
        }
        public bool GradeStudent(string courseTitle, string taskTitle, string studentLogin, int grade)
        {
            var course = EduSkyProgramm.Courses.FirstOrDefault(c => c.Title == courseTitle && c.TeacherLogin == this.Login);
            if (course == null) return false;

            var task = course.Tasks.FirstOrDefault(t => t.Title == taskTitle);
            if (task == null) return false;

            var answer = task.Answers.FirstOrDefault(a => a.StudentLogin == studentLogin);
            if (answer == null) return false;

            answer.Grade = grade;
            EduSkyProgramm.SaveAll();
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
            EduSkyProgramm.SaveAll();
            return true;
        }
    }
}
