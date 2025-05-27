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
    public partial class CourseTasksWindow : Window
    {
        private readonly RegisteredUser _currentUser;
        private readonly Course _currentCourse;

        public CourseTasksWindow(RegisteredUser currentUser, string courseTitle)
        {
            InitializeComponent();

            _currentUser = currentUser;
            _currentCourse = EduSkyProgramm.Courses.FirstOrDefault(c => c.Title == courseTitle)
                             ?? throw new Exception("Course not found");
            LoadTasks();
        }

        private void LoadTasks()
        {
            TaskStackPanel.Children.Clear();

            foreach (var task in _currentCourse.Tasks)
            {
                var stack = new StackPanel
                {
                    Orientation = Orientation.Horizontal,
                    Margin = new Thickness(0, 0, 0, 15)
                };

                var ellipse = new Ellipse
                {
                    Width = 35,
                    Height = 35,
                    Fill = new SolidColorBrush(Color.FromRgb(128, 178, 132)),
                    VerticalAlignment = VerticalAlignment.Center
                };

                var border = new Border
                {
                    BorderBrush = Brushes.Black,
                    BorderThickness = new Thickness(1),
                    CornerRadius = new CornerRadius(20),
                    Margin = new Thickness(10, 0, 0, 0),
                    Padding = new Thickness(20, 10, 20, 10),
                    Cursor = Cursors.Hand
                };

                var textBlock = new TextBlock
                {
                    Text = task.Title,
                    FontSize = 16
                };

                border.Child = textBlock;

                border.MouseLeftButtonUp += (s, e) =>
                {
                    var taskWindow = new TaskWindow(_currentUser, _currentCourse, task);
                    taskWindow.ShowDialog();
                };

                stack.Children.Add(ellipse);
                stack.Children.Add(border);

                TaskStackPanel.Children.Add(stack);
            }
        }

        private void LeaveCourse_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to leave this course?",
                                         "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                if (_currentUser.LeaveCourse(_currentCourse.Title))
                {
                    EduSkyProgramm.SaveAll();
                    MessageBox.Show("You left the course.");

                    var mainWindow = new MainPage();
                    mainWindow.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Error leaving course.");
                }
            }
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
        private void ProfileButton_Click(object sender, RoutedEventArgs e)
        {
            var profileWindow = new ProfilePage();
            profileWindow.Show();
            this.Close(); // Закрываем текущую страницу
        }
    }
}
