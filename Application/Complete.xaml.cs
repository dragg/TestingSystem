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
using System.Data;
using System.Windows.Controls.Primitives;

namespace Application
{
    /// <summary>
    /// Interaction logic for Complete.xaml
    /// </summary>
    public partial class Complete : Window
    {
        MainWindow mainWindow = null;

        private String userName = "";

        private String pathToFile = "";

        private bool _continue = false;

        private List<Question> questions = null;

        private List<bool> resultTest;

        private List<int> continueDoc = new List<int>();

        public DataTable QuestionData = new DataTable();

        public Complete()
        {
            //Инициализируем колонки таблицы
            QuestionData.Columns.Add("Number", typeof(String));

            var col = new DataColumn("Question", typeof(String));
            col.MaxLength = 200;
            QuestionData.Columns.Add(col);

            QuestionData.Columns.Add("Result", typeof(String));

            //Инициализируем остальные компоненты
            InitializeComponent();
        }

        private void SetQuestionData()
        {
            //Задаем результаты в таблицу
            for (int i = 0; i < questions.Count; i++)
            {
                var row = QuestionData.NewRow();
                QuestionData.Rows.Add(row);
                row["Number"] = i + 1;
                var question = questions[i].GetQuestion();
                row["Question"] = (question.Length > 200) ?  question.Substring(0, 200) : question;
                row["Result"] = resultTest[i] ? "Верно" : "Неверно";
            }

            //Обновляем таблицу на форме
            dataGrid.ItemsSource = QuestionData.AsDataView();
        }

        public void SetQuestionAndResult(List<Question> questions, List<bool> result, String name)
        {
            this.questions = questions;
            this.resultTest = result;
            this.userName = name;

            SetQuestionData();
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            this.Owner.Hide();

            int countRight = 0, total = 0;
            foreach (var item in resultTest)
            {
                if (item)
                {
                    countRight++;
                }
                total++;
            }
            tbResult.Text = "Вы верно квалифицировали " + countRight + " из " + total + " фабул";
        }

        private void WindowClosed(object sender, EventArgs e)
        {
            this.Owner.Hide();
            this.Owner.Show();
            this.Owner.Focus();
        }

        private bool isAnswer(int index)
        {
            foreach (var item in continueDoc)
            {
                if (item == index)
                {
                    return true;
                }
            }
            return false;
        }

        private void NextAndClose(object sender, RoutedEventArgs e)
        {
            var index = dataGrid.SelectedIndex;
            //Проверяем, что выбран вопрос и то, что он был отвечен верно
            if (index >= 0 && index < resultTest.Count && resultTest[index] && !isAnswer(index))
            {
                //дата + ФИО прогодившего + название документа
                var filename = System.IO.Path.GetFileName(questions[index].GetPathToFile());
                var date = DateTime.Now.Date.ToString().Replace('.', '-').Replace(':', '-');
                var result = date + " " + userName + " " + filename;
                pathToFile = Directory.GetCurrentDirectory() + Helper.PathToResult + result;

                continueDoc.Add(index);
                btNext.IsEnabled = false;
                //Если файл открыт, то закрываем текущее окно
                OpenFile(index);
            }
            else
            {
                MessageBox.Show("Вы должны выбрать верно отвеченный вопрос");
            }

            if ((sender as ToggleButton) != null)
            {
                (sender as ToggleButton).IsChecked = false;
            }
        }

        private void CopyFile(int index)
        {
            var directory = System.IO.Path.GetDirectoryName(pathToFile);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            questions[index].CopyFileTo(pathToFile);
        }

        private bool OpenFile(int index)
        {
            bool result = false;
            if (questions[index].CheckFile())
            {
                CopyFile(index);
                Process.Start(pathToFile);
                result = true;
            }
            else
            {
                MessageBox.Show("Файл протокола не найден.", "Ошибка", MessageBoxButton.OK);
            }
            return result;
        }

        private void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var result = MessageBox.Show("Вы действительно хотете выйти?", "Подтверждение выхода", MessageBoxButton.OKCancel, MessageBoxImage.Question);

            if (result == MessageBoxResult.OK)
            {
                this.Owner.Show();
                e.Cancel = false;
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void Close(object sender, RoutedEventArgs e)
        {
            this.Close();

            if ((sender as ToggleButton) != null)
            {
                (sender as ToggleButton).IsChecked = false;
            }
        }

        private void dataGrid_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            var index = dataGrid.SelectedIndex;

            //Проверяем, что выбран вопрос и то, что он был отвечен верно
            if (index >= 0 && index < resultTest.Count && resultTest[index] && !isAnswer(index))
            {
                btNext.IsEnabled = true;
            }
            else
            {
                btNext.IsEnabled = false;
            }
        }
    }
}
