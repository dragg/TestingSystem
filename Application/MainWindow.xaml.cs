using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Application
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void TeachersSettings(object sender, RoutedEventArgs e)
        {
            WTeacher teacherWindow = new WTeacher();
            teacherWindow.Show();
        }

        private void BeginTest(object sender, RoutedEventArgs e)
        {
            this.Hide();
            WTest wTest = new WTest();
            wTest.Owner = this;
            wTest.ShowDialog();
        }
    }
}
