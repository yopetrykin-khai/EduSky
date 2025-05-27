using Microsoft.Win32;
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
    /// <summary>
    /// Interaction logic for CreateCourseWindow.xaml
    /// </summary>
    public partial class CreateCourseWindow : Window
    {
        private string? _selectedImagePath;

        public CreateCourseWindow()
        {
            InitializeComponent();
        }

        private void AddImage_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Title = "Select an image",
                Filter = "Image files (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png"
            };

            if (dialog.ShowDialog() == true)
            {
                _selectedImagePath = dialog.FileName;
                MessageBox.Show("Image selected successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(_selectedImagePath);
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();
                CourseImage.Source = bitmap;
            }
        }

        private void CreateCourse_Click(object sender, RoutedEventArgs e)
        {
            var title = CourseNameBox.Text.Trim();

            if (string.IsNullOrEmpty(title) || title == "Enter the name of the your course")
            {
                MessageBox.Show("Please enter a course name.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var teacher = EduSkyProgramm.CurrentUser as Teacher;
            if (teacher == null)
            {
                MessageBox.Show("Only teachers can create courses.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (CourseNameBox.Text.Length < 3)
            {
                MessageBox.Show("Course`s name should have more than 6 chars.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            string imagePath = _selectedImagePath ?? "";
            bool success = teacher.CreateCourse(title, imagePath);
            if (success)
            {
                MessageBox.Show("Course created successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                Close();
            }
            else
            {
                MessageBox.Show("Course with this name already exists.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
