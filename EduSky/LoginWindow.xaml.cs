using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace EduSky
{
    public partial class LoginWindow : Window
    {
        private readonly AuthService _authService;

        public LoginWindow()
        {
            InitializeComponent();
            _authService = new AuthService(EduSkyProgramm.UserDataService);
            if (EduSkyProgramm.Users.Count == 0 || EduSkyProgramm.Courses.Count == 0)
                EduSkyProgramm.LoadData(EduSkyProgramm.UserDataService, EduSkyProgramm.CourseDataService);
        }
        private void SignInButton_Click(object sender, RoutedEventArgs e)
        {
            var login = LoginBox.Text.Trim();
            var password = PasswordBox.Password.Trim();

            var user = _authService.Login(login, password);

            if (user != null)
            {
                EduSkyProgramm.CurrentUser = user;
                if (user is Teacher)
                {
                    var TeacherCourses = new TeacherCourses();
                    TeacherCourses.Show();
                    this.Close();
                }
                else if (user is RegisteredUser)
                {
                    var mainPage = new MainPage();
                    mainPage.Show();
                    this.Close();
                }               
            }
            else
            {
                MessageBox.Show("Wrong login or password", "ERROR", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void SignUpButton_Click(object sender, RoutedEventArgs e)
        {
            var RegisterWindow = new RegisterWindow();
            RegisterWindow.Show();
            this.Close();
        }
    }
}
