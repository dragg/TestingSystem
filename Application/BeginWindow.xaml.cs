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

namespace Application
{
    /// <summary>
    /// Interaction logic for BeginWindow.xaml
    /// </summary>
    public partial class BeginWindow : Window
    {
        public BeginWindow()
        {
            InitializeComponent();
        }

        private void ProtocolWindows(object sender, RoutedEventArgs e)
        {
            MainWindow window = new MainWindow();
            window.Owner = this;
            this.Hide();
            window.Show();
        }
    }
}
