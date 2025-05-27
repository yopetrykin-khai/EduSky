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
    public partial class RegisterWindow : Window
    {
        private readonly AuthService _authService;

        public RegisterWindow()
        {
            InitializeComponent();
            _authService = new AuthService(EduSkyProgramm.UserDataService);
        }

        private void SignUp_Click(object sender, RoutedEventArgs e)
        {
            var name = NameBox.Text.Trim();
            var login = EmailBox.Text.Trim();
            var password = PasswordBox.Password.Trim();

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please fill out all the fields.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var selectedRole = UserRole.Student;
            foreach (var child in ((StackPanel)((StackPanel)NameBox.Parent).Children[6]).Children)
            {
                if (child is RadioButton rb && rb.IsChecked == true && rb.Content.ToString() == "Teacher")
                {
                    selectedRole = UserRole.Teacher;
                }
            }

            try
            {
                var guest = new Guest
                {
                    Name = name,
                    Login = login,
                    Password = password
                };

                var user = guest.Register(selectedRole);

                if (_authService.Register(user))
                {
                    EduSkyProgramm.CurrentUser = user;
                    var mainWindow = new MainWindow();
                    mainWindow.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("A user with such a login already exists.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Registration failed:\n{ex.Message}", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void SignIn_Click(object sender, RoutedEventArgs e)
        {
            var loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }
    }
}
