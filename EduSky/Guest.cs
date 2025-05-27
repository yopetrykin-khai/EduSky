using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduSky
{

    public class Guest : User
    {
        public override string GetRole() => "Guest";

        public User Register(UserRole role)
        {
            return role switch
            {
                UserRole.Teacher => new Teacher
                {
                    Login = this.Login,
                    Password = this.Password,
                    Name = this.Name,
                    GraduatedFrom = this.GraduatedFrom
                },
                UserRole.Student => new RegisteredUser
                {
                    Login = this.Login,
                    Password = this.Password,
                    Name = this.Name,
                    GraduatedFrom = this.GraduatedFrom
                },
                _ => throw new ArgumentException("Невідома роль")
            };
        }
    }

}
