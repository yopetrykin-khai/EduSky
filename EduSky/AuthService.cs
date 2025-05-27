using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduSky
{
    public class AuthService
    {
        private readonly IDataService<User> _userService;

        public AuthService(IDataService<User> userService)
        {
            _userService = userService;
        }

        public User? Login(string login, string password)
        {
            return EduSkyProgramm.Users.FirstOrDefault(u => u.Login == login && u.Password == password);
        }

        public bool Register(User newUser)
        {
            if (EduSkyProgramm.Users.Any(u => u.Login == newUser.Login)) return false;

            EduSkyProgramm.Users.Add(newUser);
            _userService.Save(EduSkyProgramm.Users); // локальне збереження
            EduSkyProgramm.SaveData(_userService, new JsonDataService<Course>("courses.json")); // глобальне
            return true;
        }
    }

}
