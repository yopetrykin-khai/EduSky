using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduSky
{
    public static class EduSkyProgramm
    {
        public static List<User> Users { get; set; } = new();
        public static List<Course> Courses { get; set; } = new();
        public static User? CurrentUser { get; set; }

        public static void LoadData(IDataService<User> userService, IDataService<Course> courseService)
        {
            Users = userService.Load();
            Courses = courseService.Load();
        }

        public static void SaveData(IDataService<User> userService, IDataService<Course> courseService)
        {
            userService.Save(Users);
            courseService.Save(Courses);
        }
    }
}
