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
    public partial class MainPage : Window
    {
        public MainPage()
        {
            InitializeComponent();
            CoursesPanel.Items.Clear();

            if (EduSkyProgramm.CurrentUser is not RegisteredUser student)
                return;

            var enrolledCourses = EduSkyProgramm.Courses
                .Where(c => student.EnrolledCourseTitles.Contains(c.Title))
                .Take(6)
                .ToList();

            if (enrolledCourses.Count == 0)
            {
                NoCoursesMessage.Visibility = Visibility.Visible;
                CoursesPanel.Visibility = Visibility.Collapsed;
            }
            else
            {
                NoCoursesMessage.Visibility = Visibility.Collapsed;
                CoursesPanel.Visibility = Visibility.Visible;

                foreach (var course in enrolledCourses)
                    CoursesPanel.Items.Add(CreateCourseCard(course, isMyCourse: true));
            }
        }

        private void MyCourses_Click(object sender, RoutedEventArgs e)
        {
            CoursesPanel.Items.Clear();

            if (EduSkyProgramm.CurrentUser is not RegisteredUser student)
                return;

            var enrolledCourses = EduSkyProgramm.Courses
                .Where(c => student.EnrolledCourseTitles.Contains(c.Title))
                .Take(6)
                .ToList();

            if (enrolledCourses.Count == 0)
            {
                NoCoursesMessage.Visibility = Visibility.Visible;
                CoursesPanel.Visibility = Visibility.Collapsed;
            }
            else
            {
                NoCoursesMessage.Visibility = Visibility.Collapsed;
                CoursesPanel.Visibility = Visibility.Visible;

                foreach (var course in enrolledCourses)
                    CoursesPanel.Items.Add(CreateCourseCard(course, isMyCourse: true));
            }
        }

        private void AllCourses_Click(object sender, RoutedEventArgs e)
        {
            CoursesPanel.Items.Clear();

            if (EduSkyProgramm.CurrentUser is not RegisteredUser student)
                return;

            var availableCourses = EduSkyProgramm.Courses
                .Where(c => !student.EnrolledCourseTitles.Contains(c.Title))
                .ToList();

            if (availableCourses.Count == 0)
            {
                NoCoursesMessage.Visibility = Visibility.Visible;
                CoursesPanel.Visibility = Visibility.Collapsed;
            }
            else
            {
                NoCoursesMessage.Visibility = Visibility.Collapsed;
                CoursesPanel.Visibility = Visibility.Visible;

                foreach (var course in availableCourses)
                    CoursesPanel.Items.Add(CreateCourseCard(course, isMyCourse: false));
            }
        }


        private Border CreateCourseCard(Course course, bool isMyCourse)
        {
            var image = new Image
            {
                Width = 80,
                Height = 80,
                Margin = new Thickness(0, 10, 0, 10),
                Source = new BitmapImage(new Uri(course.ImagePath, UriKind.RelativeOrAbsolute))
            };

            var title = new TextBlock
            {
                Text = course.Title,
                FontSize = 14,
                Margin = new Thickness(0, 10, 0, 10),
                TextAlignment = TextAlignment.Center
            };

            var button = new Button
            {
                Width = 100,
                Height = 30,
                Content = isMyCourse ? "Open" : "Sign up",
                Background = new SolidColorBrush(Color.FromRgb(47, 62, 70)),
                Foreground = Brushes.White,
                HorizontalAlignment = HorizontalAlignment.Center,
                Tag = course
            };

            if (!isMyCourse)
            {
                button.Click += SignUpToCourse;
            }
            else
            {
                button.Click += (s, e) =>
                {
                    if (EduSkyProgramm.CurrentUser is RegisteredUser student)
                    {
                        var courseWindow = new CourseTasksWindow(student, course.Title);
                        courseWindow.Show();
                        this.Close(); 
                    }
                };
            }

            var panel = new StackPanel { HorizontalAlignment = HorizontalAlignment.Center };
            panel.Children.Add(image);
            panel.Children.Add(title);
            panel.Children.Add(button);

            return new Border
            {
                BorderBrush = Brushes.Black,
                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(20),
                Padding = new Thickness(10),
                Width = 200,
                Height = 260,
                Margin = new Thickness(10),
                Child = panel
            };
        }

        private void SignUpToCourse(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is Course course && EduSkyProgramm.CurrentUser is RegisteredUser user)
            {
                bool success = user.EnrollToCourse(course.Title);
                if (success)
                {
                    MessageBox.Show($"You signed up for {course.Title}!");
                    EduSkyProgramm.SaveAll();
                    AllCourses_Click(null, null); 
                }
                else
                {
                    MessageBox.Show("Could not sign up.");
                }
            }
        }

        private void ProfileButton_Click(object sender, RoutedEventArgs e)
        {
            var profileWindow = new ProfilePage();
            profileWindow.Show();
            this.Close(); 
        }
    }

}
