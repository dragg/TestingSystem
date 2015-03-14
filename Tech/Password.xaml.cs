using CommonLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Application
{
    /// <summary>
    /// Interaction logic for Password.xaml
    /// </summary>
    public partial class Password : Window
    {
        private string password = "";

        public Password()
        {
            InitializeComponent();
        }

        private void ToggleButton_Click_1(object sender, RoutedEventArgs e)
        {
            if (this.password != "" && this.password == pbPassword.Password)
            {
                var window = new BeginWindow();
                window.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Неверный пароль для доступа!", "Ошибка доступа", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            pbPassword.Focus();
            try
            {
                string temp = "";
                StreamReader reader = new StreamReader(Helper.PathToSettings);

                reader.ReadLine();
                reader.ReadLine();
                temp = reader.ReadLine();
                password = Crypting.Decrypt(temp, Helper.Key);
                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Невозможно прочитать файл с настройками!", "Ошибка доступа", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            if ((sender as ToggleButton) != null)
            {
                (sender as ToggleButton).IsChecked = false;
            }
        }
    }
}
