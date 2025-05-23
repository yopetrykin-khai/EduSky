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
        public override string GetRole() => throw new NotImplementedException();

        public bool CreateCourse(string title, string description)
        {
            throw new NotImplementedException();
        }

        public bool AddTaskToCourse(string courseTitle, string taskTitle, string taskDescription, string filePath)
        {
            throw new NotImplementedException();
        }
    }
}
