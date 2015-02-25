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

namespace Application
{
    /// <summary>
    /// Interaction logic for WInforming.xaml
    /// </summary>
    public partial class WInforming : Window
    {
        private bool result = false;

        private bool note2 = false;

        private String note = "";

        public WInforming()
        {
            InitializeComponent();
        }

        private void OkAndClose(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public void SaveInformation(bool result, String note, bool note2 = false)
        {
            this.note2 = note2;
            this.result = result;
            this.note = note;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (!note2)
            {
                tbStatus.Text = result ? "Вы ответили верно!" : "Вы ошиблись!";
            }
            else
            {
                tbStatus.Text = "";
            }
            tbNote.Text = note;
        }
    }
}
