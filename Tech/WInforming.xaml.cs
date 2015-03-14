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
                tbNote.Text = result ? "" : note;
                if (result)
                {
                    this.Width = 300;
                    this.Height = 150;
                    tbNote.Width = 0;
                    tbNote.Height = 0;
                }
                else
                {
                    this.Width = 600;
                    this.Height = 400;
                }
            }
            else
            {
                tbStatus.Text = "КоАП";
                this.Title = "КоАП";
                tbNote.Text = note;
                this.Width = 600;
                this.Height = 400;
            }

            //Позиционируем после того как утсановили разрешение окна
            this.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;

            double height = SystemParameters.FullPrimaryScreenHeight,
                width = SystemParameters.FullPrimaryScreenWidth;

            this.Top = (height - this.Height) / 2;
            this.Left = (width - this.Width) / 2;
        }
    }
}
