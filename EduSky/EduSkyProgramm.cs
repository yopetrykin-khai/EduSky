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
        public static IDataService<User> UserDataService => new JsonDataService<User>("users.json");
        public static IDataService<Course> CourseDataService => new JsonDataService<Course>("courses.json");

        public static void SaveAll()
        {
            UpdateCurrentUserInList();
            SaveData(UserDataService, CourseDataService);
        }

        private static void UpdateCurrentUserInList()
        {
            if (CurrentUser == null) return;
            var index = Users.FindIndex(u => u.Login == CurrentUser.Login);
            if (index != -1)
                Users[index] = CurrentUser;
        }
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
