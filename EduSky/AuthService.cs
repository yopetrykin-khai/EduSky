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
            throw new NotImplementedException();
        }

        public User? Login(string login, string password)
        {
            throw new NotImplementedException();
        }

        public bool Register(User newUser)
        {
            throw new NotImplementedException();
        }
    }

}
