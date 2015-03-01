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
using System.IO;
using System.Diagnostics;

namespace Application
{
    /// <summary>
    /// Interaction logic for Complete.xaml
    /// </summary>
    public partial class Complete : Window
    {
        MainWindow mainWindow = null;

        private String pathToFile = "";

        private List<Question> questions = null;

        private List<Tuple<List<bool>, List<bool>>> wasAnswerAndHow;

        public Complete()
        {
            InitializeComponent();
        }

        public void setWindow(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
        }

        public void setQuestionAndResult(List<Question> questions, List<Tuple<List<bool>, List<bool>>> wasAnswerAndHow)
        {
            this.questions = questions;
            this.wasAnswerAndHow = wasAnswerAndHow;
        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            this.Owner.Hide();
        }

        private void Window_Closed_1(object sender, EventArgs e)
        {
            this.Owner.Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            pathToFile = Directory.GetCurrentDirectory() + Helper.PathToResult + "1.docx";

            OpenFile();
        }

        private void CopyFile()
        {
            questions[0].CopyFileTo(pathToFile);
        }

        private bool OpenFile()
        {
            if (questions[0].CheckFile())
            {
                CopyFile();
                Process.Start(pathToFile);
            }
            else
            {
                MessageBox.Show("Файл протокола не найден.\nОбратитесь к преподавателю!", "Ошибка", MessageBoxButton.OK);
            }
            return true;
        }
    }
}
