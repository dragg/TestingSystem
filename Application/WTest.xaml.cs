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
using System.IO;

namespace Application
{
    /// <summary>
    /// Interaction logic for WTest.xaml
    /// </summary>
    public partial class WTest : Window
    {
        private List<Question> questions = new List<Question>();

        public WTest()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            bool fail = false;
            try
            {
                StreamReader reader = new StreamReader(Helper.DefaultPathToFileWithQuestion);
                String countQuestion = reader.ReadLine();
                int cnt = Int32.Parse(countQuestion);
                for (int i = 0; i < cnt; i++)
                {
                    Question q = new Question();
                    q.ReadQuestion(reader);
                    questions.Add(q);
                }
                reader.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Ошибка при чтении вопросов. Возможно нет исходного файла");
                fail = true;
            }

            if (questions.Count == 0)
            {
                MessageBox.Show("Не загружено ни одного вопроса! Проверьте настройки.");
                fail = true;
            }

            if (fail)
            {
                this.Owner.Show();
                this.Close();
            }
        }
    }
}
