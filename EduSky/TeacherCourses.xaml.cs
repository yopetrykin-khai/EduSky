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
    public partial class TeacherCourses : Window
    {
        private Teacher _teacher;

        public TeacherCourses()
        {
            InitializeComponent();
            _teacher = EduSkyProgramm.CurrentUser as Teacher
                ?? throw new InvalidOperationException("Current user is not a teacher.");
            LoadTeacherCourses();
        }

        private void LoadTeacherCourses()
        {
            CoursesPanel.Children.Clear();

            var courses = EduSkyProgramm.Courses
                .Where(c => c.TeacherLogin == _teacher.Login)
                .ToList();

            foreach (var course in courses)
            {
                var courseCard = CreateCourseCard(course); // передаём весь объект
                CoursesPanel.Children.Add(courseCard);
            }

            CoursesPanel.Children.Add(CreateAddCourseCard());
        }

        private Border CreateCourseCard(Course course)
        {
            var image = new Image
            {
                Source = new BitmapImage(new Uri(course.ImagePath, UriKind.RelativeOrAbsolute)),
                Width = 110,
                Height = 110,
                Margin = new Thickness(0, 20, 0, 10)
            };

            var titleBlock = new TextBlock
            {
                Text = course.Title,
                FontSize = 14,
                TextAlignment = TextAlignment.Center,
                TextWrapping = TextWrapping.Wrap
            };

            var button = new Button
            {
                Content = "Open course",
                Width = 100,
                Margin = new Thickness(0, 20, 0, 0),
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2C3E50")),
                Foreground = Brushes.White
            };

            button.Click += (s, e) =>
            {
                var courseWindow = new TasksTeacherWindow(course); 
                courseWindow.ShowDialog();
                LoadTeacherCourses();
            };

            var stack = new StackPanel
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                Children = { image, titleBlock, button }
            };

            return new Border
            {
                BorderBrush = Brushes.Black,
                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(20),
                Width = 150,
                Height = 220,
                Margin = new Thickness(10),
                Child = stack
            };
        }

        private Border CreateAddCourseCard()
        {
            var plusText = new TextBlock
            {
                Text = "+",
                FontSize = 48,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0, 40, 0, 20)
            };

            var button = new Button
            {
                Content = "Create course",
                Width = 120,
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2C3E50")),
                Foreground = Brushes.White
            };

            button.Click += (s, e) =>
            {
                var createWindow = new CreateCourseWindow();
                if (createWindow.ShowDialog() == true)
                {
                    LoadTeacherCourses();
                }
            };

            var stack = new StackPanel
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                Children = { plusText, button }
            };

            return new Border
            {
                BorderBrush = Brushes.Black,
                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(20),
                Width = 150,
                Height = 220,
                Margin = new Thickness(10),
                Child = stack
            };
        }

        private void ProfileButton_Click(object sender, RoutedEventArgs e)
        {
            var profileWindow = new ProfilePage();
            profileWindow.Show();
            this.Close();
        }
    }
}
