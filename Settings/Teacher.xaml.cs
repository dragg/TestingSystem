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
using CommonLibrary;
using System.Windows.Controls.Primitives;

namespace Settings
{
    /// <summary>
    /// Interaction logic for Teacher.xaml
    /// </summary>
    public partial class WTeacher : Window
    {
        private bool Saved = false;

        private bool Changed = false;

        private List<Question> listQuestions = new List<Question>();

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
            for (int i = 0; i < listQuestions.Count; i++)
            {
                var row = QuestionData.NewRow();
                QuestionData.Rows.Add(row);
                row["Number"] = i + 1;
                String question = listQuestions[i].GetQuestion();
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
            if ((sender as ToggleButton) != null)
            {
                (sender as ToggleButton).IsChecked = false;
            }
        }

        private void AddQuestion(object sender, RoutedEventArgs e)
        {
            WQuestion wQuestion = new WQuestion();
            wQuestion.Owner = this;
            wQuestion.ShowDialog();
            Changed = true;
            if ((sender as ToggleButton) != null)
            {
                (sender as ToggleButton).IsChecked = false;
            }
        }

        private void ChangeQuestion(object sender, RoutedEventArgs e)
        {
            ChangeQuestion();
            if ((sender as ToggleButton) != null)
            {
                (sender as ToggleButton).IsChecked = false;
            }
        }

        private void DeleteQuestion(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Вы действительно хотите удалить вопрос?", "Удаление вопроса", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if(result == MessageBoxResult.Yes)
            {
                DeleteQuestion();
            }

            if ((sender as ToggleButton) != null)
            {
                (sender as ToggleButton).IsChecked = false;
            }
        }

        private void lbListQuestions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var index = lbListQuestions.SelectedIndex;
            if (index != -1 && index < listQuestions.Count)
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
            if (index != -1 && index < listQuestions.Count)
            {
                listQuestions.RemoveAt(index);
            }
            ShowQuestion();
        }

        private void ChangeQuestion()
        {
            var index = lbListQuestions.SelectedIndex;
            if (index != -1)
            {
                Question q = listQuestions[index] as Question;
                WQuestion wQuestion = new WQuestion(q);
                wQuestion.Owner = this;
                wQuestion.ShowDialog();
            }
            ShowQuestion();
        }

        public void AddQuestion(Question q)
        {
            listQuestions.Add(q);
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
                        //listQuestions = new List<CommonLibrary.Question>(cnt);
                        for (int i = 0; i < cnt; i++)
                        {
                            Question q = new Question();
                            q.ReadQuestion(read_text);
                            listQuestions.Add(q);
                        }
                        read_text.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    
                }

                ShowQuestion();
            }
            if ((sender as ToggleButton) != null)
            {
                (sender as ToggleButton).IsChecked = false;
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

                write_text.WriteLine(Crypting.Encrypt(listQuestions.Count.ToString(), Helper.Key));
                foreach (var question in listQuestions)
                {
                    question.WriteQuestion(write_text);
                }
                write_text.Close();
            }
            if ((sender as ToggleButton) != null)
            {
                (sender as ToggleButton).IsChecked = false;
            }
        }

        private void DeleteAllQuestion(object sender, RoutedEventArgs e)
        {
            listQuestions.Clear();
            ShowQuestion();
            Changed = true;
            if ((sender as ToggleButton) != null)
            {
                (sender as ToggleButton).IsChecked = false;
            }
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

                (sender as ToggleButton).IsChecked = false;
            }
            catch(Exception ex)
            {
                MessageBox.Show("Не удалось сохранить настройки!", "Ошибка", MessageBoxButton.OK);
            }

            if ((sender as ToggleButton) != null)
            {
                (sender as ToggleButton).IsChecked = false;    
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
            listQuestions[index] = q;
        }
    }
}
