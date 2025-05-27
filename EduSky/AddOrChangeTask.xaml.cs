// AddOrChangeTask.xaml.cs
using Microsoft.Win32;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace EduSky
{
    public partial class AddOrChangeTask : Window
    {
        private Task _task;
        private Teacher _teacher;
        private StudentAnswer? _selectedAnswer;
        private bool isEditing = false;
        public AddOrChangeTask(Task task, Teacher teacher)
        {
            InitializeComponent();
            _task = task;
            _teacher = teacher;

            TaskTitleBox.Text = _task.Title;
            TaskDescriptionBox.Text = _task.Description;
            StudentSelector.ItemsSource = _task.Answers.Select(a => a.StudentLogin);
            StudentSelector.SelectedIndex = 0;

            GradeTextBox.TextChanged += GradeTextBox_TextChanged;
            UpdateGradePlaceholder();
        }
        private void EditTask_Click(object sender, RoutedEventArgs e)
        {
            if (!isEditing)
            {
                TaskTitleBox.IsReadOnly = false;
                TaskDescriptionBox.IsReadOnly = false;
                TaskTitleBox.BorderBrush = Brushes.Black;
                TaskDescriptionBox.BorderBrush = Brushes.Black;

                TaskTitleBox.Background = Brushes.White;
                TaskDescriptionBox.Background = Brushes.White;

                isEditing = true;
            }
            else
            {
                SaveChanges_Click(sender, e);
                isEditing = false;
                TaskTitleBox.IsReadOnly = true;
                TaskDescriptionBox.IsReadOnly = true;
                TaskTitleBox.Background = Brushes.Transparent;
                TaskDescriptionBox.Background = Brushes.Transparent;
            }
        }
        private void GradeTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateGradePlaceholder();
        }

        private void UpdateGradePlaceholder()
        {
            foreach (var child in ((Grid)GradeTextBox.Parent).Children)
            {
                if (child is TextBlock placeholder && placeholder.Text.StartsWith("Enter grade"))
                {
                    placeholder.Visibility = string.IsNullOrWhiteSpace(GradeTextBox.Text) ? Visibility.Visible : Visibility.Collapsed;
                }
            }
        }

        private void StudentSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var login = StudentSelector.SelectedItem as string;
            _selectedAnswer = _task.Answers.FirstOrDefault(a => a.StudentLogin == login);
            StudentFileText.Text = _selectedAnswer?.FilePath != null
                ? "📄 " + Path.GetFileName(_selectedAnswer.FilePath)
                : "📄 No file selected";
            if (_selectedAnswer?.Grade != null)
            {
                GradeTextBox.Text = _selectedAnswer.Grade.ToString();
            }
            else
            {
                GradeTextBox.Text = string.Empty;
            }
            UpdateGradePlaceholder();
        }

        private void DownloadMethodical_Click(object sender, RoutedEventArgs e)
        {
            if (!isEditing)
            {
                if (File.Exists(_task.MethodicalFilePath))
                {
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = _task.MethodicalFilePath,
                        UseShellExecute = true
                    });
                }
                else
                {
                    MessageBox.Show("File not found.");
                }
            }
            else
            {
                var dialog = new OpenFileDialog
                {
                    Filter = "PDF files (*.pdf)|*.pdf|All files (*.*)|*.*"
                };

                if (dialog.ShowDialog() == true)
                {
                    _task.MethodicalFilePath = dialog.FileName;
                    DownloadMethodicalButton.Content = "📄 " + dialog.FileName.ToString();
                }
            }
        }

        private void DownloadStudentFile_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedAnswer != null && File.Exists(_selectedAnswer.FilePath))
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = _selectedAnswer.FilePath,
                    UseShellExecute = true
                });
            }
            else
            {
                MessageBox.Show("No file to open.");
            }
        }

        private void GradeStudent_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedAnswer == null)
            {
                MessageBox.Show("Select a student.");
                return;
            }

            if (int.TryParse(GradeTextBox.Text, out int grade) && grade >= 1 && grade <= 10)
            {
                _selectedAnswer.Grade = grade;
                MessageBox.Show($"Grade {grade} saved.");
                EduSkyProgramm.SaveAll();
            }
            else
            {
                MessageBox.Show("Enter valid grade (1-10).");
            }
        }

        private void SaveChanges_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _task.Title = TaskTitleBox.Text;
                _task.Description = TaskDescriptionBox.Text;
                EduSkyProgramm.SaveAll();
                MessageBox.Show("Changes saved.");
                DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void DeleteTask_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to delete this task?", "Confirm", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                foreach (var course in EduSkyProgramm.Courses)
                {
                    if (course.Tasks.Contains(_task))
                    {
                        course.Tasks.Remove(_task);
                        break;
                    }
                }
                EduSkyProgramm.SaveAll();
                Close();
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}