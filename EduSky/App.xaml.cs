using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace EduSky
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static AuthService AuthService { get; private set; }

        private readonly IDataService<User> _userDataService = new JsonDataService<User>("users.json");
        private readonly IDataService<Course> _courseDataService = new JsonDataService<Course>("courses.json");

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Завантаження даних
            EduSkyProgramm.LoadData(_userDataService, _courseDataService);

            // Ініціалізація сервісу авторизації
            AuthService = new AuthService(_userDataService);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);

            // Збереження даних при виході
            EduSkyProgramm.SaveData(_userDataService, _courseDataService);
        }
    }
}
