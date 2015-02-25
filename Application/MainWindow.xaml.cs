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
using CommonLibrary;

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
            //mainGrid.Height = this.Height;
            //mainGrid.Width = this.Width;
        }

        private void TeachersSettings(object sender, RoutedEventArgs e)
        {
            WTeacher teacherWindow = new WTeacher();
            teacherWindow.Show();
        }

        private void BeginTest(object sender, RoutedEventArgs e)
        {
            if (FIO.Text != "")
            {
                this.Hide();
                WTest wTest = new WTest();
                wTest.Owner = this;
                try
                {
                    wTest.ShowDialog();
                }
                catch (Exception ex)
                {
                    //Заплатка
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, заполните данные!");
            }
            
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
                btSetting.IsEnabled = !btSetting.IsEnabled;
        }
    }
}
