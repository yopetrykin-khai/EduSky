using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduSky
{
    public abstract class User
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? Name { get; set; }
        public string? GraduatedFrom { get; set; }
        public virtual string GetProfileInfo()
        {
            return $"Ім'я: {Name}\nОсвіта: {GraduatedFrom}\nРоль: {GetRole()}";
        }

        public abstract string GetRole();
    }
}
