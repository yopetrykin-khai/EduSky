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
    public partial class ProfilePage : Window
    {
        private bool isEditing = false;
        public ProfilePage()
        {
            InitializeComponent();
            LoadUserInfo();
        }
        private void LoadUserInfo()
        {
            if (EduSkyProgramm.CurrentUser is null)
            {
                MessageBox.Show("No user is currently logged in.");
                return;
            }
            txtB_name.Text = EduSkyProgramm.CurrentUser.Name ?? "Not set";
            txtB_date.Text = EduSkyProgramm.CurrentUser.BirthDate?.ToString("dd/MM/yyyy") ?? "Not set";
            txtB_education.Text = EduSkyProgramm.CurrentUser.GraduatedFrom ?? "Not set";
            txtB_Role.Text = EduSkyProgramm.CurrentUser.GetRole();
        }
        private void SignOut_Click(object sender, RoutedEventArgs e)
        {
            EduSkyProgramm.SaveAll();
            var currentUser = EduSkyProgramm.CurrentUser;
            EduSkyProgramm.CurrentUser = null;
                var mainPage = new MainWindow();
                mainPage.Show();
            this.Close();
        }
        private void GoBack_Click(object sender, RoutedEventArgs e)
        {
            if (EduSkyProgramm.CurrentUser is RegisteredUser)
            {
                new MainPage().Show();
            }
            else if (EduSkyProgramm.CurrentUser is Teacher)
            {
                new TeacherCourses().Show();
            }
            this.Close();
        }
        private void EditProfile_Click(object sender, RoutedEventArgs e)
        {
            isEditing = true;
            txtB_name.IsReadOnly = false;
            txtB_date.IsReadOnly = false;
            txtB_education.IsReadOnly = false;
            btnSave.Visibility = Visibility.Visible;
        }
        private void SaveProfile_Click(object sender, RoutedEventArgs e)
        {
            if (EduSkyProgramm.CurrentUser is null) return;

            if (!DateTime.TryParse(txtB_date.Text, out DateTime parsedDate))
            {
                MessageBox.Show("Invalid date format. Use dd/MM/yyyy.");
                return;
            }

            try
            {
                EduSkyProgramm.CurrentUser.Name = txtB_name.Text;
                EduSkyProgramm.CurrentUser.BirthDate = parsedDate;
                EduSkyProgramm.CurrentUser.GraduatedFrom = txtB_education.Text;

                EduSkyProgramm.SaveAll();

                txtB_name.IsReadOnly = true;
                txtB_date.IsReadOnly = true;
                txtB_education.IsReadOnly = true;

                btnSave.Visibility = Visibility.Collapsed;
                isEditing = false;

                MessageBox.Show("Profile updated successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Update failed:\n{ex.Message}", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

    }
}
