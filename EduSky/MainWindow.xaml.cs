﻿using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EduSky
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var courses = EduSkyProgramm.Courses.Take(6).ToList();

            if (courses.Any())
            {
                CoursesPanel.ItemsSource = courses;
                NoCoursesMessage.Visibility = Visibility.Collapsed;
            }
            else
            {
                NoCoursesMessage.Visibility = Visibility.Visible;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }
    }
}
