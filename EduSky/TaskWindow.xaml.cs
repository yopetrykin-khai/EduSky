using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
    /// Interaction logic for TaskWindow.xaml
    /// </summary>
    public partial class TaskWindow : Window
    {
        private RegisteredUser _currentUser;
        private Course _currentCourse;
        private EduSky.Task _currentTask;
        private StudentAnswer _studentAnswer;
        public TaskWindow(RegisteredUser currentUser, Course course, EduSky.Task task)
        {
            InitializeComponent();
            _currentUser = currentUser;
            _currentCourse = course;
            _currentTask = task;

            TitleTextBlock.Text = task.Title;
            TextBlockName.Text = task.Title;
            DescriptionTextBlock.Text = task.Description;

            if (!string.IsNullOrEmpty(task.MethodicalFilePath))
            {
                DownloadButton.Visibility = Visibility.Visible;
            }

            UpdateSubmittedFileInfo();
            _studentAnswer = _currentTask.Answers.FirstOrDefault(a => a.StudentLogin == _currentUser.Login);

            if (_studentAnswer != null && _studentAnswer.Grade.HasValue)
            {
                GradeTextBlock.Text = $"Grade: {_studentAnswer.Grade}";
            }
            else
            {
                GradeTextBlock.Text = "Grade: not graded yet";
            }
        }
        private void DownloadButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(_currentTask.MethodicalFilePath) && File.Exists(_currentTask.MethodicalFilePath))
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = _currentTask.MethodicalFilePath,
                    UseShellExecute = true
                });
            }
            else
            {
                MessageBox.Show("File not found.");
            }
        }
        private void AddOrCreate_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                _currentUser.SubmitAnswer(_currentCourse.Title, _currentTask.Title, openFileDialog.FileName);
                UpdateSubmittedFileInfo();
            }
        }
        private void SendAgain_Click(object sender, RoutedEventArgs e)
        {
            AddOrCreate_Click(sender, e);
        }
        private void UpdateSubmittedFileInfo()
        {
            var answer = _currentTask.Answers.FirstOrDefault(a => a.StudentLogin == _currentUser.Login);
            if (answer != null)
            {
                SubmittedFileText.Text = $"📎 {System.IO.Path.GetFileName(answer.FilePath)}";
            }
            else
            {
                SubmittedFileText.Text = "No file submitted yet.";
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
