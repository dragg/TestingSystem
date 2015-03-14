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
            var now = DateTime.Now;
            day.Text = now.Day.ToString();

            String _month = "";
            switch (now.Month)
            {
                case 1:
                    _month = "Январь";
                    break;
                case 2:
                    _month = "Февраль";
                    break;
                case 3:
                    _month = "Март";
                    break;
                case 4:
                    _month = "Апрель";
                    break;
                case 5:
                    _month = "Май";
                    break;
                case 6:
                    _month = "Июнь";
                    break;
                case 7:
                    _month = "Июль";
                    break;
                case 8:
                    _month = "Август";
                    break;
                case 9:
                    _month = "Сертябрь";
                    break;
                case 10:
                    _month = "Октябрь";
                    break;
                case 11:
                    _month = "Ноябрь";
                    break;
                case 12:
                    _month = "Декабрь";
                    break;
                default:
                    break;
            }
            month.Text = _month;
            year.Text = (now.Year - 2000).ToString();

            FIO.Focus();
        }

        private void TeachersSettings(object sender, RoutedEventArgs e)
        {
        }

        private void BeginTest(object sender, RoutedEventArgs e)
        {
            if (FIO.Text != "")
            {
                this.Hide();
                WTest wTest = new WTest();
                wTest.SetUserName(FIO.Text);
                wTest.Owner = this.Owner;
                try
                {
                    wTest.ShowDialog();
                    this.Close();
                }
                catch (Exception ex)
                {
                    //Заплатка
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, заполните данные!");
                FIO.Focus();
            }

            if ((sender as ToggleButton) != null)
            {
                (sender as ToggleButton).IsChecked = false;
            }
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
        }

        private void WindowClosed(object sender, EventArgs e)
        {
            if (this.Owner != null)
            {
                this.Owner.Show();
            }
        }
    }
}
