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
    /// Interaction logic for Complete.xaml
    /// </summary>
    public partial class Complete : Window
    {
        MainWindow mainWindow = null;

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
    }
}
