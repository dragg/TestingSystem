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

        private List<Question> questions = null;

        private List<bool> resultTest;

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
                row["Question"] = questions[i].GetQuestion();
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
        }

        private void WindowClosed(object sender, EventArgs e)
        {
            this.Owner.Show();
        }

        private void NextAndClose(object sender, RoutedEventArgs e)
        {
            var index = dataGrid.SelectedIndex;
            //Проверяем, что выбран вопрос и то, что он был отвечен верно
            if (index >= 0 && index < resultTest.Count && resultTest[index])
            {
                //дата + ФИО прогодившего + название документа
                var filename = System.IO.Path.GetFileName(questions[index].GetPathToFile());
                var date = DateTime.Now.ToString().Replace('.', '-').Replace(':', '-');
                var result = date + userName + filename;
                pathToFile = Directory.GetCurrentDirectory() + " " + Helper.PathToResult + " " + result;

                //Если файл открыт, то закрываем текущее окно
                if (OpenFile(index))
                {
                    (this.Owner as MainWindow).FIO.Text = "";
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("Вы должны выбрать верно отвеченный вопрос");
            }
            
        }

        private void CopyFile(int index)
        {
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
    }
}
