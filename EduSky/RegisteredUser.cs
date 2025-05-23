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
        public override string GetRole() => throw new NotImplementedException();

        public List<Task> ViewTasks(string courseTitle)
        {
            throw new NotImplementedException();
        }

        public bool SubmitAnswer(string courseTitle, string taskTitle, string filePath)
        {
            throw new NotImplementedException();
        }

        public bool EnrollToCourse(string courseTitle)
        {
            throw new NotImplementedException();
        }

        public bool LeaveCourse(string courseTitle)
        {
            throw new NotImplementedException();
        }
    }
}
