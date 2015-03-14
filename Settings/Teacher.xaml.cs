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
using Microsoft.Win32;
using System.IO;
using CommonLibrary;
using System.Data;

namespace Settings
{
    /// <summary>
    /// Interaction logic for Teacher.xaml
    /// </summary>
    public partial class WTeacher : Window
    {
        private bool Saved = false;

        private bool Changed = false;

        //private List<Question> listQuestions;// = new List<Question>();

        //Используем массив, а не лист потому что возникла страннейшая неразрешимая ошибка 
        //Создаем массив из 1000 потенциальных вопросов
        private Question[] listOfQuestion = new Question[1000];
        private int countQuestion = 0;

        public DataTable QuestionData = new DataTable();

        public WTeacher()
        {
            //Инициализируем колонки таблицы
            QuestionData.Columns.Add("Number", typeof(String));

            var col = new DataColumn("Question", typeof(String));
            col.MaxLength = 200;
            QuestionData.Columns.Add(col);

            //Инициализируем остальные компоненты
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //listQuestions = new List<Question>();
            FileInfo settings = new FileInfo(Helper.PathToSettings);
            bool exist = true;
            string password = "";
            if (settings.Exists == false)
            {
                StreamWriter writer = settings.AppendText();
                writer.WriteLine(Helper.DefaultCountQuestion);
                writer.WriteLine(Helper.DefaultPathToFileWithQuestion);
                
                writer.Close();
                exist = false;
            }

            int countQuestion;
            String path;
            if (!exist)
            {
                countQuestion = Helper.DefaultCountQuestion;
                path = Helper.DefaultPathToFileWithQuestion;
            }
            else
            {
                StreamReader reader = new StreamReader(Helper.PathToSettings);

                string temp = "";

                temp = reader.ReadLine();
                countQuestion = Int32.Parse(Crypting.Decrypt(temp, Helper.Key));

                temp = reader.ReadLine();
                path = Crypting.Decrypt(temp, Helper.Key);

                temp = reader.ReadLine();
                password = Crypting.Decrypt(temp, Helper.Key);

                //countQuestion = Int32.Parse(reader.ReadLine());
                //path = reader.ReadLine();
                //password = reader.ReadLine();
                reader.Close();
            }

            tbCountQuestion.Text = countQuestion.ToString();
            tbPathToFile.Text = path;
            pbPassword.Password = password;
            
        }

        private void ShowQuestion()
        {
            //Очищаем предыдущие данные
            QuestionData.Rows.Clear();

            //Задаем результаты в таблицу
            for (int i = 0; i < this.countQuestion; i++)
            {
                var row = QuestionData.NewRow();
                QuestionData.Rows.Add(row);
                row["Number"] = i + 1;
                String question = listOfQuestion[i].GetQuestion();
                row["Question"] = question.Substring(0, (question.Length > 120 ? 120 : question.Length));
            }

            //Обновляем таблицу на форме
            lbListQuestions.ItemsSource = QuestionData.AsDataView();


            //lbListQuestions.Items.Clear();
            //foreach (var question in listQuestions)
            //{
            //    lbListQuestions.Items.Add(question);
            //}
            Changed = true;
        }

        private void OpenSelectQuestion(object sender, MouseButtonEventArgs e)
        {
            ChangeQuestion();
            Changed = true;
        }

        private void AddQuestion(object sender, RoutedEventArgs e)
        {
            WQuestion wQuestion = new WQuestion();
            wQuestion.Owner = this;
            wQuestion.ShowDialog();
            Changed = true;
        }

        private void ChangeQuestion(object sender, RoutedEventArgs e)
        {
            ChangeQuestion();
        }

        private void DeleteQuestion(object sender, RoutedEventArgs e)
        {
            DeleteQuestion();
        }

        private void lbListQuestions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var index = lbListQuestions.SelectedIndex;
            if (index != -1 && index < this.countQuestion)
            {
                btChangeQuestion.IsEnabled = btDeleteQuestion.IsEnabled = true;
            }
            else
            {
                btChangeQuestion.IsEnabled = btDeleteQuestion.IsEnabled = false;
            }
        }

        private void DeleteQuestion()
        {
            var index = lbListQuestions.SelectedIndex;
            if (index != -1 && index < this.countQuestion)
            {
                //TODO: Добавить удаление вопроса
                //listQuestions.RemoveAt(index);
            }
            ShowQuestion();
        }

        private void ChangeQuestion()
        {
            var index = lbListQuestions.SelectedIndex;
            if (index != -1)
            {
                Question q = listOfQuestion[index] as Question;
                WQuestion wQuestion = new WQuestion(q);
                wQuestion.Owner = this;
                wQuestion.ShowDialog();
            }
            ShowQuestion();
        }

        public void AddQuestion(Question q)
        {
            listOfQuestion[this.countQuestion++] = q;
            ShowQuestion();
        }

        private void OpenQuestions(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fDiaglog = new OpenFileDialog();
            fDiaglog.DefaultExt = ".dat";
            fDiaglog.FileName = "";
            fDiaglog.Filter = "Data files (.dat)|*.dat";

            if (fDiaglog.ShowDialog() == true)
            {
                String fileName = fDiaglog.FileName;

                FileInfo file = new FileInfo(fileName);
                if (file.Exists == true)
                {
                    int cnt = 0;
                    try
                    {
                        StreamReader read_text = new StreamReader(fileName);
                        String countQuestion = Crypting.Decrypt(read_text.ReadLine(), Helper.Key);

                        cnt = Int32.Parse(countQuestion);
                        //listOfQuestion = new Question[cnt];
                        //listQuestions = new List<CommonLibrary.Question>(cnt);
                        //MessageBox.Show("before read questions file");
                        for (int i = 0; i < cnt; i++)
                        {
                            Question q = new Question();
                            q.ReadQuestion(read_text);
                            //MessageBox.Show("before add to list");
                            //listQuestions.Insert(i, q);
                            listOfQuestion[i] = q;
                            //var list = new List<Question>();
                            //list.Add(q);
                            //listQuestions.Add(q);
                            //MessageBox.Show("after add to list");
                        }
                        read_text.Close();
                        //MessageBox.Show("Before to list");
                        //var listQuestions = listOfQuestion.<int, Question>();
                        //MessageBox.Show("after close file");
                    }
                    catch (Exception ex)
                    {
                        //MessageBox.Show(ex.Data.ToString());
                        //MessageBox.Show(cnt.ToString());
                        MessageBox.Show(ex.Message);
                    }
                    
                }

                ShowQuestion();
            }
        }

        private void SaveQuestions(object sender, RoutedEventArgs e)
        {
            SaveFileDialog fDiaglog = new SaveFileDialog();
            fDiaglog.DefaultExt = ".dat";
            fDiaglog.FileName = "";
            fDiaglog.Filter = "Data files (.dat)|*.dat";

            if (fDiaglog.ShowDialog() == true)
            {
                String fileName = fDiaglog.FileName;

                FileInfo file = new FileInfo(fileName);
                if (file.Exists == true)
                {
                    file.Delete();
                }


                StreamWriter write_text;
                write_text = file.AppendText();

                write_text.WriteLine(Crypting.Encrypt(this.countQuestion.ToString(), Helper.Key));
                foreach (var question in listOfQuestion)
                {
                    question.WriteQuestion(write_text);
                }
                write_text.Close();
            }
        }

        private void DeleteAllQuestion(object sender, RoutedEventArgs e)
        {
            //TODO: добавить удаление всех вопросов
            //listQuestions.Clear();
            ShowQuestion();
            Changed = true;
        }

        private void ApplySettings(object sender, RoutedEventArgs e)
        {
            try
            {
                int cnt = Int32.Parse(tbCountQuestion.Text);
                String path = tbPathToFile.Text;

                FileInfo settings = new FileInfo(Helper.PathToSettings);
                if(settings.Exists)
                    settings.Delete();
                StreamWriter writer = settings.AppendText();
                writer.WriteLine(Crypting.Encrypt(cnt.ToString(), Helper.Key));
                writer.WriteLine(Crypting.Encrypt(path, Helper.Key));
                writer.WriteLine(Crypting.Encrypt(pbPassword.Password, Helper.Key));
                //writer.WriteLine(cnt);
                //writer.WriteLine(path);
                //writer.WriteLine(pbPassword.Password);
                writer.Close();
                MessageBox.Show("Настройки успешно сохранены!", "Успех", MessageBoxButton.OK);
            }
            catch(Exception ex)
            {
                MessageBox.Show("Не удалось сохранить настройки!", "Ошибка", MessageBoxButton.OK);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (Changed && !Saved)
            {
                NotSaverChange wNSC = new NotSaverChange();
                if (!(wNSC.ShowDialog() == true))
                {
                    e.Cancel = true;
                }
            }
        }

        public void ChangeQuestion(Question q)
        {
            var index = lbListQuestions.SelectedIndex;
            listOfQuestion[index] = q;
        }
    }
}
