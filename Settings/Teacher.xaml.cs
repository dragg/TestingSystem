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

        public WTeacher()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            FileInfo settings = new FileInfo(Helper.PathToSettings);
            bool exist = true;
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
                countQuestion = Int32.Parse(reader.ReadLine());
                path = reader.ReadLine();
                reader.Close();
            }

            tbCountQuestion.Text = countQuestion.ToString();
            tbPathToFile.Text = path;
            
        }

        private void ShowQuestion()
        {
            lbListQuestions.Items.Clear();
            foreach (var question in listQuestions)
            {
                lbListQuestions.Items.Add(question);
            }
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
            if (lbListQuestions.SelectedIndex != -1)
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
            if (lbListQuestions.SelectedIndex != -1)
            {
                listQuestions.RemoveAt(lbListQuestions.SelectedIndex);
            }
            ShowQuestion();
        }

        private void ChangeQuestion()
        {
            if (lbListQuestions.SelectedIndex != -1)
            {
                Question q = listQuestions[lbListQuestions.SelectedIndex] as Question;
                DeleteQuestion();
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
                    StreamReader read_text = new StreamReader(fileName);
                    String countQuestion = read_text.ReadLine();
                    int cnt = Int32.Parse(countQuestion);
                    for (int i = 0; i < cnt; i++)
                    {
                        Question q = new Question();
                        q.ReadQuestion(read_text);
                        listQuestions.Add(q);
                    }
                    read_text.Close();
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

                write_text.WriteLine(listQuestions.Count);
                foreach (var question in listQuestions)
                {
                    question.WriteQuestion(write_text);
                }
                write_text.Close();
            }
        }

        private void DeleteAllQuestion(object sender, RoutedEventArgs e)
        {
            listQuestions.Clear();
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
                writer.WriteLine(cnt);
                writer.WriteLine(path);
                writer.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Не удалось применить. Проверьте вверность вводимых данных!");
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
    }
}
