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
using System.Windows.Shapes;
using CommonLibrary;

namespace Settings
{
    /// <summary>
    /// Interaction logic for NotSaverChange.xaml
    /// </summary>
    public partial class NotSaverChange : Window
    {
        public NotSaverChange()
        {
            InitializeComponent();
        }

        private void NotSaveAndClose(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            this.Close();
        }

        private void ReturnBackAndClose(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }
    }
}
