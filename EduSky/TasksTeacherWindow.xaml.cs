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
    public partial class TasksTeacherWindow : Window
    {
        private Course _course;
        private Teacher _teacher;

        public TasksTeacherWindow(Course course)
        {
            InitializeComponent();
            _course = course;
            _teacher = EduSkyProgramm.CurrentUser as Teacher
                       ?? throw new InvalidOperationException("Current user is not a teacher.");
            LoadTasks();
        }
        private void LoadTasks()
        {
            TaskPanel.Children.Clear();

            foreach (var task in _course.Tasks)
            {
                var taskStack = new StackPanel
                {
                    Orientation = Orientation.Horizontal,
                    Margin = new Thickness(0, 10, 0, 0)
                };

                var ellipse = new Ellipse
                {
                    Width = 30,
                    Height = 30,
                    Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#80B284")),
                    VerticalAlignment = VerticalAlignment.Center
                };
                var button = new Button
                {
                    Content = $"{task.Title}",
                    Width = 300,
                    Height = 50,
                    Margin = new Thickness(10, 0, 0, 0),
                    HorizontalContentAlignment = HorizontalAlignment.Left,
                    BorderBrush = Brushes.Black,
                    BorderThickness = new Thickness(1),
                    Background = Brushes.White,
                    Tag = task
                };
                button.Click += (s, e) =>
                {
                    var editTaskWindow = new AddOrChangeTask(task, _teacher); 
                    editTaskWindow.ShowDialog();
                    LoadTasks(); 
                };
                taskStack.Children.Add(ellipse);
                taskStack.Children.Add(button);
                TaskPanel.Children.Add(taskStack);
            }
            var addStack = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Margin = new Thickness(0, 20, 0, 0)
            };
            var addEllipse = new Ellipse
            {
                Width = 30,
                Height = 30,
                Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#80B284")),
                VerticalAlignment = VerticalAlignment.Center
            };
            var addButton = new Button
            {
                Content = "+ Add new task",
                Width = 300,
                Height = 50,
                Margin = new Thickness(10, 0, 0, 0),
                HorizontalContentAlignment = HorizontalAlignment.Left,
                BorderBrush = Brushes.Black,
                BorderThickness = new Thickness(1),
                Background = Brushes.White
            };
            addButton.Click += AddTask_Click;
            addStack.Children.Add(addEllipse);
            addStack.Children.Add(addButton);
            TaskPanel.Children.Add(addStack);
        }
        private void AddTask_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var newTask = new Task
                {
                    Title = "New Task",
                    Description = "Write description here",
                    Answers = new List<StudentAnswer>(),
                    MethodicalFilePath = ""
                };
                var addTaskWindow = new AddOrChangeTask(newTask, _teacher);
                var result = addTaskWindow.ShowDialog();
                if (result == true)
                {
                    _course.Tasks.Add(newTask);
                    EduSkyProgramm.SaveAll();
                }
                LoadTasks();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void DeleteCourse_Click(object sender, MouseButtonEventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to delete this course?", "Confirm Deletion", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                EduSkyProgramm.Courses.Remove(_course);
                foreach (var user in EduSkyProgramm.Users)
                {
                    if (user is RegisteredUser student)
                    {
                        student.EnrolledCourseTitles.Remove(_course.Title);
                    }
                    else if (user is Teacher teacher)
                    {
                        teacher.CreatedCourseTitles.Remove(_course.Title);
                    }
                }
                EduSkyProgramm.SaveAll();
                MessageBox.Show("Course deleted.");
                Close(); 
            }
        }
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


    }
}
