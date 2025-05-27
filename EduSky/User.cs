using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduSky
{
    public abstract class User
    {
        private string _login;
        private string _password;
        private DateTime? _birthDate;
        public string Login
        {
            get => _login;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new Exception("Login cannot be empty.");
                if (value.Length < 3 || value.Length > 16)
                    throw new Exception("Login must be between 3 and 16 characters.");
                _login = value;
            }
        }
        public string Password
        {
            get => _password;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new Exception("Password cannot be empty.");
                if (value.Length > 16)
                    throw new Exception("Password cannot be longer than 16 characters.");
                if (!value.All(char.IsLetterOrDigit))
                    throw new Exception("Password can contain only letters and digits.");

                _password = value;
            }
        }
        public DateTime? BirthDate
        {
            get => _birthDate;
            set
            {
                if (value.HasValue && value.Value > DateTime.Now)
                    throw new Exception("Birth date cannot be in the future.");
                _birthDate = value;
            }
        }
        public string? Name { get; set; }
        public string? GraduatedFrom { get; set; }
        public virtual string GetProfileInfo()
        {
            return $"Ім'я: {Name}\nОсвіта: {GraduatedFrom}\nРоль: {GetRole()}";
        }
        public abstract string GetRole();
    }
}