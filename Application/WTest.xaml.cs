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
        private List<Tuple <List<bool>, List<bool>>> wasAnswerAndHow;

        private int right = 0, wrong = 0;

        private int countQuestion;

        private int currentQuestion = 0;

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
                StreamReader reader = new StreamReader(Helper.PathToSettings);
                this.countQuestion = Int32.Parse(reader.ReadLine());
                String path = reader.ReadLine();
                reader.Close();

                List<Question> AllQuestions = new List<Question>();
                reader = new StreamReader(path);
                int countQuestion = Int32.Parse(reader.ReadLine());
                for (int i = 0; i < countQuestion; i++)
                {
                    Question q = new Question();
                    q.ReadQuestion(reader);
                    AllQuestions.Add(q);
                }
                reader.Close();


                List<bool> used = new List<bool>();
                for (int i = 0; i < AllQuestions.Count; i++)
                {
                    used.Add(false);
                }

                if (AllQuestions.Count < this.countQuestion)
                {
                    throw new Exception("Question error!");
                }
                else
                {
                    Random rnd = new Random();
                    int max = AllQuestions.Count;
                    for (int find = this.countQuestion; 0 < find; find--)
                    {
                        int now = rnd.Next(0, max);
                        while (used[now])
                        {
                            now = rnd.Next(0, max);
                        }
                        used[now] = true;

                        questions.Add(AllQuestions[now]);
                    }
                }
            }
            catch (Exception ex)
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
            else
            {

                wasAnswerAndHow = new List<Tuple<List<bool>, List<bool>>>();
                foreach (var q in questions)
                {
                    List<bool> answers = new List<bool>();
                    for (int i = 0; i < q.GetAllAnswer().Count; i++)
                    {
                        answers.Add(false);
                    }
                    List<bool> right = new List<bool>();
                    right.Add(false);
                    wasAnswerAndHow.Add(new Tuple<List<bool>, List<bool>>(right, answers));
                }

                ShowQuestion();
            }
        }

        private void Next(object sender, RoutedEventArgs e)
        {
            if (wasAnswerAndHow[currentQuestion + 1].Item1[0])
            {
                btNote.IsEnabled = true;
            }
            else
            {
                SaveValues();//Сохраняем отмеченные значения
                btNote.IsEnabled = false;
            }

            DeleteQuestion();//Убираем отображение старого вопроса
            NextQuestion();//Показываем новый вопрос
        }

        private void Prev(object sender, RoutedEventArgs e)
        {
            if (wasAnswerAndHow[currentQuestion - 1].Item1[0])
            {
                btNote.IsEnabled = true;
            }
            else
            {
                SaveValues();//Сохраняем отмеченные значения
                btNote.IsEnabled = false;
            }

            DeleteQuestion();//Убираем отображение старого вопроса
            PrevQuestion();//Показываем новый вопрос
        }

        private void Finish(object sender, RoutedEventArgs e)
        {
            StreamWriter writer;
            FileInfo info = new FileInfo(Helper.PathToMembers);
            
            if (!info.Exists)
            {
                info.Create().Close();
                
                writer = info.AppendText();
                writer.WriteLine("ФИО\tВсего вопросов\tПравильных ответов\tНеверных ответов");
                writer.Close();
            }
            writer = info.AppendText();
            writer.WriteLine("{3}\t{0}\t{1}\t{2}", countQuestion, right, wrong, (this.Owner as MainWindow).FIO.Text);
            writer.Close();

            MessageBox.Show(String.Format("Ваш результат:\nВерных ответов:{0}\nНеверных ответов:{1}", right, wrong));

            this.Owner.Show();
            this.Close();
        }

        private void ShowNote(object sender, RoutedEventArgs e)
        {
            WInforming wInformation = new WInforming();
            wInformation.Owner = this;
            wInformation.SaveInformation(IsRightAnswer(), questions[currentQuestion].GetNote());
            wInformation.ShowDialog();
        }

        private void ToAnswer(object sender, RoutedEventArgs e)
        {
            wasAnswerAndHow[currentQuestion].Item1[0] = true;//Помечаем, что на вопрос ответили
            SaveValues();

            WInforming wInformation = new WInforming();
            wInformation.Owner = this;

            bool error = !IsRightAnswer();


            if (error)
            {
                wrong++;
            }
            else
            {
                right++;
            }

            wInformation.SaveInformation(error ? false : true, questions[currentQuestion].GetNote());
            wInformation.ShowDialog();

            DisableAllCheckBox();

            if (CheckAllAnswer())
                btFinish.IsEnabled = true;

            btNote.IsEnabled = true;
            btToAnswer.IsEnabled = false;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            bool AllAnswers = true;
            foreach (var item in wasAnswerAndHow)
            {
                if (item.Item1[0] == false)
                {
                    
                    AllAnswers = false;
                    break;
                }
            }

            if (AllAnswers)
            {
                //Finish(sender, null);
            }
            else
            {
                //ShowDialog();
                //MessageBox.("Вы действительно хотете выйти? Только кнопка Завершить сохранит все данные!");
            }
            this.Owner.Show();
            this.Close();
        }

        private void DisableAllCheckBox()
        {
            foreach (var answer in spAnswers.Children)
            {
                if (answer is StackPanel)
                {
                    foreach (var item in (answer as StackPanel).Children)
                    {
                        if (item is CheckBox)
                        {
                            (item as CheckBox).IsEnabled = false;
                        }
                    }
                }
            }
        }

        private void SetValues()
        {
            int i = 0;
            foreach (var answer in spAnswers.Children)
            {
                if (answer is StackPanel)
                {
                    foreach (var item in (answer as StackPanel).Children)
                    {
                        if (item is CheckBox)
                        {
                            (item as CheckBox).IsChecked = wasAnswerAndHow[currentQuestion].Item2[i++];
                        }
                    }
                }
            }
        }

        private void NextQuestion()
        {
            if (currentQuestion < countQuestion - 1)
            {
                currentQuestion++;
            }

            if (currentQuestion == countQuestion - 1)
            {
                btNext.IsEnabled = false;
            }

            if (!btPrev.IsEnabled)
            {
                btPrev.IsEnabled = true;
            }


            ShowQuestion();
        }

        private void PrevQuestion()
        {
            if (currentQuestion > 0)
            {
                currentQuestion--;
            }

            if (currentQuestion == 0)
            {
                btPrev.IsEnabled = false;
            }

            if (!btNext.IsEnabled)
            {
                btNext.IsEnabled = true;
            }

            ShowQuestion();
        }

        private void ShowQuestion()
        {
            List<StackPanel> listAnswers = new List<StackPanel>();
            List<Answer> answers = questions[currentQuestion].GetAllAnswer();
            for (int i = 0; i < answers.Count; i++)
            {
                StackPanel spAnswer = new StackPanel();
                spAnswer.Orientation = Orientation.Horizontal;
                CheckBox chTrue = new CheckBox();
                chTrue.Margin = new Thickness(10, 5, 5, 5);

                TextBlock tbAnswer = new TextBlock();
                tbAnswer.Text = answers[i].Text;
                tbAnswer.Margin = new Thickness(10, 5, 5, 5);

                spAnswer.Children.Add(chTrue);
                spAnswer.Children.Add(tbAnswer);

                listAnswers.Add(spAnswer);
            }

            tbQuestion.Text = questions[currentQuestion].GetQuestion();

            foreach (var spAnswer in listAnswers)
            {
                spAnswers.Children.Add(spAnswer);
            }

            SetValues();
            if (wasAnswerAndHow[currentQuestion].Item1[0])
            {
                DisableAllCheckBox();
                btToAnswer.IsEnabled = false;
            }
            else
            {
                btToAnswer.IsEnabled = true;
            }
        }

        private void DeleteQuestion()
        {
            int count = spAnswers.Children.Count;
            for (int i = count - 1; i >= 0; i--)
            {
                spAnswers.Children.RemoveAt(i);
            }

            tbQuestion.Text = "";
        }

        private void SaveValues()
        {
            int i = 0;
            foreach (var item in spAnswers.Children)
            {
                StackPanel spAnswer = item as StackPanel;
                int cnt = spAnswer.Children.Count;
                foreach (var checkBox in spAnswer.Children)
                {
                    if (checkBox is CheckBox)
                    {
                        bool check = ((checkBox as CheckBox).IsChecked == true ? true : false);
                        wasAnswerAndHow[currentQuestion].Item2[i++] = check;
                    }
                }
            }
        }

        private bool CheckAllAnswer()
        {
            return (right + wrong == countQuestion);
        }

        private bool IsRightAnswer()
        {
            try
            {
                List<Answer> listAnswers = questions[currentQuestion].GetAllAnswer();
                for (int i = 0; i < listAnswers.Count; i++)
                {
                    if (wasAnswerAndHow[currentQuestion].Item2[i] != listAnswers[i].Right)
                    {
                        throw new Exception("Неверный ответ!");
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
    }
}
