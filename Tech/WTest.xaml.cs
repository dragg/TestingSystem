﻿using System;
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
using CommonLibrary;
using System.IO; 
using System.Text; 
using System.Security.Cryptography;
using System.Diagnostics;
using System.Windows.Controls.Primitives;

namespace Application
{
    /// <summary>
    /// Interaction logic for WTest.xaml
    /// </summary>
    public partial class WTest : Window
    {
        private Question currentQuestionAnswers = null;

        private String UserName = "";

        private bool closing_now = false;

        private bool finish = false;

        private List<bool> resultTest = new List<bool>();

        private List<bool> secondAttampt = new List<bool>();

        DateTime begin;

        //Для каждого вопроса каждому последовательно ответу задается соответственный параметр:
        //                                                                              1 - был ли отвечен вопрос
        //                                                                              2 - на каждый ответ, как был отвечен
        private List<Tuple <List<bool>, List<bool>>> wasAnswerAndHow;

        private List<int> checkedCounters = new List<int>();

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
                string temp;
                StreamReader reader = new StreamReader(Helper.PathToSettings);

                temp = reader.ReadLine();
                this.countQuestion = Int32.Parse(Crypting.Decrypt(temp, Helper.Key));

                temp = reader.ReadLine();
                String path = Crypting.Decrypt(temp, Helper.Key);

                reader.Close();

                List<Question> AllQuestions = new List<Question>();
                reader = new StreamReader(path);
                int countQuestion = Int32.Parse(Crypting.Decrypt(reader.ReadLine(), Helper.Key));
                if (countQuestion == 1)
                {
                    btNext.IsEnabled = false;
                }
                for (int i = 0; i < countQuestion; i++)
                {
                    resultTest.Add(false);
                    secondAttampt.Add(false);
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
                    throw new Exception("Количество вопросов меньше, чем задано для теста!");
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

                    checkedCounters.Add(0);
                }
                //ShowQuestion();
            }
        }

        private void Next(object sender, RoutedEventArgs e)
        {
            SaveValues();//Сохраняем отмеченные значения
            if (wasAnswerAndHow[currentQuestion + 1].Item1[0])
            {
                btNote.IsEnabled = true;
            }
            else
            {
                //btNote.IsEnabled = false;
            }

            DeleteQuestion();//Убираем отображение старого вопроса
            NextQuestion();//Показываем новый вопрос

            if ((sender as ToggleButton) != null)
            {
                (sender as ToggleButton).IsChecked = false;
            }
        }

        private void Prev(object sender, RoutedEventArgs e)
        {
            SaveValues();//Сохраняем отмеченные значения
            if (wasAnswerAndHow[currentQuestion - 1].Item1[0])
            {
                btNote.IsEnabled = true;
            }
            else
            {
                //btNote.IsEnabled = false;
            }

            DeleteQuestion();//Убираем отображение старого вопроса
            PrevQuestion();//Показываем новый вопрос

            if ((sender as ToggleButton) != null)
            {
                (sender as ToggleButton).IsChecked = false;
            }
        }

        private void Finish(object sender, RoutedEventArgs e)
        {
            if (!CheckAllAnswer())
            {
                String message = "У вас не квалифицицированы следующие фабулы: ";
                bool first = true;
                for (int i = 0; i < wasAnswerAndHow.Count; i++)
                {
                    if (!wasAnswerAndHow[i].Item1[0])
                    {
                        message += (first ? "" : ", ") + (i + 1);
                        first = false;
                    }
                }
                message += "!";
                MessageBox.Show(message);
            }
            else
            {
                finish = true;
                StreamWriter writer;
                FileInfo info = new FileInfo(Helper.PathToMembers);
                int countSecondAttampt = 0;
                foreach (var item in secondAttampt)
                {
                    countSecondAttampt += item ? 1 : 0;
                }
                if (!info.Exists)
                {
                    info.Create().Close();

                    writer = info.AppendText();
                    writer.WriteLine("ФИО\t\t\t\t\tВсего фабул\tПравильных ответов\tНеверных ответов\tВторых попыток\tДата\t\tВремя в минутах");
                    writer.Close();
                }
                writer = info.AppendText();
                String str = "{3}";
                for (int i = 0; i < 5 - UserName.Length / 8; i++)
                {
                    str += "\t";
                }
                writer.WriteLine(str + "{0}\t\t{1}\t\t\t{2}\t\t\t{6}\t\t{4}\t{5}", countQuestion, right, wrong, UserName, DateTime.Now.Date.ToShortDateString(), (int)((DateTime.Now - begin).TotalMinutes), countSecondAttampt);
                writer.Close();

                //MessageBox.Show(String.Format("Ваш результат:\nВерных ответов:{0}\nНеверных ответов:{1}", right, wrong));

                

                Complete complete = new Complete();
                complete.Owner = this.Owner;
                complete.SetQuestionAndResult(questions, resultTest, UserName);
                complete.Show();

                if (!closing_now)
                {
                    this.Close();
                }
            }

            if ((sender as ToggleButton) != null)
            {
                (sender as ToggleButton).IsChecked = false;
            }
        }

        private void ShowNote(object sender, RoutedEventArgs e)
        {
            WInforming wInformation = new WInforming();
            wInformation.Owner = this;
            wInformation.SaveInformation(IsRightAnswer(), questions[currentQuestion].GetNote());
            wInformation.ShowDialog();

            if ((sender as ToggleButton) != null)
            {
                (sender as ToggleButton).IsChecked = false;
            }
        }

        private void ToAnswer(object sender, RoutedEventArgs e)
        {
            //Проверим, что в каждой категории выбран ответ
            if (checkedCounters[currentQuestion] != 4)
            {
                MessageBox.Show("Выберите в каждой категории ответ!");
            }
            else
            {
                if (secondAttampt[currentQuestion] == false)
                {
                    if (!CheckAnswerAndShowResult())
                    {
                        secondAttampt[currentQuestion] = true;    
                    }
                }
                else
                {
                    CheckAnswerAndShowResult(false);
                }
            }

            if ((sender as ToggleButton) != null)
            {
                (sender as ToggleButton).IsChecked = false;
            }
        }

        private bool CheckAnswerAndShowResult(bool first = true)
        {
            bool result = false;

            SaveValues();

            WInforming wInformation = new WInforming();
            wInformation.Owner = this;

            bool error = !IsRightAnswer();
            if (!error && first || !first)
            {
                wasAnswerAndHow[currentQuestion].Item1[0] = true;//Помечаем, что на вопрос ответили
                if (error)
                {
                    wrong++;
                }
                else
                {
                    right++;
                    result = true;
                }
            }

            wInformation.SaveInformation(error ? false : true, questions[currentQuestion].GetNote());
            wInformation.ShowDialog();

            if (!error && first || !first)
            {
                DisableAllCheckBox();

                //Now always is enabled!
                //if (CheckAllAnswer())
                //    btFinish.IsEnabled = true;

                btNote.IsEnabled = true;
                btToAnswer.IsEnabled = false;

                ShowStatus();
            }

            return result;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            
        }

        private void DisableAllCheckBox()
        {
            foreach (var answer in spObjectAnswers.Children)
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

            foreach (var answer in spSubjectAnswers.Children)
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

            foreach (var answer in spObjectiveSideAnswers.Children)
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

            foreach (var answer in spSubjectiveSideAnswers.Children)
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
            checkedCounters[currentQuestion] = 0;
            int i = 0;
            foreach (var answer in spObjectAnswers.Children)
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

            foreach (var answer in spObjectiveSideAnswers.Children)
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


            foreach (var answer in spSubjectAnswers.Children)
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


            foreach (var answer in spSubjectiveSideAnswers.Children)
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
            List<StackPanel> listObjectAnswers = new List<StackPanel>();
            List<StackPanel> listSubjectAnswers = new List<StackPanel>();
            List<StackPanel> listObjectiveSideAnswers = new List<StackPanel>();
            List<StackPanel> listSubjectiveSideAnswers = new List<StackPanel>();
            List<Answer> answers = questions[currentQuestion].GetAllAnswer();
            var width = (mainWindow.Width - 600 - 100) / 2 - 50;
            for (int i = 0; i < answers.Count; i++)
            {
                StackPanel spAnswer = new StackPanel();
                spAnswer.Orientation = Orientation.Horizontal;
                CheckBox chTrue = new CheckBox();
                chTrue.Margin = new Thickness(10, 5, 5, 5);

                TextBlock tbAnswer = new TextBlock();
                tbAnswer.Width = width;
                
                tbAnswer.TextWrapping = TextWrapping.Wrap;
                tbAnswer.Text = answers[i].Text;
                tbAnswer.Margin = new Thickness(10, 5, 5, 5);

                spAnswer.Children.Add(chTrue);
                spAnswer.Children.Add(tbAnswer);

                if (answers[i].Subject == "Объект")
                {
                    chTrue.Checked += new RoutedEventHandler(this.CheckBoxChangedObject);
                    chTrue.Unchecked += new RoutedEventHandler(this.CheckBoxChangedObject);
                    listObjectAnswers.Add(spAnswer);
                }
                else if (answers[i].Subject == "Субъект")
                {
                    chTrue.Checked += new RoutedEventHandler(this.CheckBoxChangedSubject);
                    chTrue.Unchecked += new RoutedEventHandler(this.CheckBoxChangedSubject);
                    listSubjectAnswers.Add(spAnswer);
                }
                else if (answers[i].Subject == "Субъектная сторона")
                {
                    chTrue.Checked += new RoutedEventHandler(this.CheckBoxChangedSubjectiveSide);
                    chTrue.Unchecked += new RoutedEventHandler(this.CheckBoxChangedSubjectiveSide);
                    listSubjectiveSideAnswers.Add(spAnswer);
                }
                else if (answers[i].Subject == "Объктная сторона")
                {
                    chTrue.Checked += new RoutedEventHandler(this.CheckBoxChangedObjectiveSide);
                    chTrue.Unchecked += new RoutedEventHandler(this.CheckBoxChangedObjectiveSide);
                    listObjectiveSideAnswers.Add(spAnswer);
                }
                //listAnswers.Add(spAnswer);
            }

            tbQuestion.Text = questions[currentQuestion].GetQuestion();
            //tbQuestion.Width = this.Width - 15;
            //tbQuestion.TextWrapping = TextWrapping.Wrap;

            foreach (var spAnswer in listObjectAnswers)
            {
                spObjectAnswers.Children.Add(spAnswer);
            }
            foreach (var spAnswer in listSubjectAnswers)
            {
                spSubjectAnswers.Children.Add(spAnswer);
            }
            foreach (var spAnswer in listSubjectiveSideAnswers)
            {
                spSubjectiveSideAnswers.Children.Add(spAnswer);
            }
            foreach (var spAnswer in listObjectiveSideAnswers)
            {
                spObjectiveSideAnswers.Children.Add(spAnswer);
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

            ShowStatus();
        }

        private void ShowStatus()
        {
            tbAnwerStatus.Text = (wasAnswerAndHow[currentQuestion].Item1[0] == false) ? ("Квалификация не осуществлена") : ("Квалификация осуществлена " + (IsRightAnswer() ? "верно" : "неверно"));
            tbPageStatus.Text = "Фабула №" + (currentQuestion + 1) + " из " + countQuestion + "";
        }

        private void CheckBoxChangedObject(object sender, RoutedEventArgs e)
        {
            bool IsChecked = (sender as CheckBox).IsChecked == true;
            foreach (var item in spObjectAnswers.Children)
            {
                if (IsChecked)
                {
                    BlockOtherCheckBox(sender, item);
                }
                else
                {
                    ReleaseAllCheckBox(item);
                }
            }
            CheckedCounter(IsChecked);
        }

        private void CheckedCounter(bool IsChecked)
        {
            if (IsChecked)
            {
                checkedCounters[currentQuestion]++;
            }
            else
            {
                checkedCounters[currentQuestion]--;
            }
        }

        private static void ReleaseAllCheckBox(object item)
        {
            StackPanel spAnswer = item as StackPanel;
            int cnt = spAnswer.Children.Count;
            foreach (var itemBox in spAnswer.Children)
            {
                if (itemBox is CheckBox)
                {
                    (itemBox as CheckBox).IsEnabled = true;
                }
            }
        }

        private static void BlockOtherCheckBox(object sender, object item)
        {
            StackPanel spAnswer = item as StackPanel;
            int cnt = spAnswer.Children.Count;
            foreach (var itemBox in spAnswer.Children)
            {
                if (itemBox is CheckBox)
                {
                    if (itemBox != sender)
                    {
                        (itemBox as CheckBox).IsEnabled = false;
                    }
                }
            }
        }

        private void CheckBoxChangedObjectiveSide(object sender, RoutedEventArgs e)
        {
            bool IsChecked = (sender as CheckBox).IsChecked == true;
            foreach (var item in spObjectiveSideAnswers.Children)
            {
                if (IsChecked)
                {
                    BlockOtherCheckBox(sender, item);
                }
                else
                {
                    ReleaseAllCheckBox(item);
                }
            }
            CheckedCounter(IsChecked);
        }

        private void CheckBoxChangedSubject(object sender, RoutedEventArgs e)
        {
            bool IsChecked = (sender as CheckBox).IsChecked == true;
            foreach (var item in spSubjectAnswers.Children)
            {
                if (IsChecked)
                {
                    BlockOtherCheckBox(sender, item);
                }
                else
                {
                    ReleaseAllCheckBox(item);
                }
            }
            CheckedCounter(IsChecked);
        }

        private void CheckBoxChangedSubjectiveSide(object sender, RoutedEventArgs e)
        {
            bool IsChecked = (sender as CheckBox).IsChecked == true;
            foreach (var item in spSubjectiveSideAnswers.Children)
            {
                if (IsChecked)
                {
                    BlockOtherCheckBox(sender, item);
                }
                else
                {
                    ReleaseAllCheckBox(item);
                }
            }
            CheckedCounter(IsChecked);
        }

        private void DeleteQuestion()
        {
            int count = spObjectAnswers.Children.Count;
            for (int i = count - 1; i >= 0; i--)
            {
                spObjectAnswers.Children.RemoveAt(i);
            }

            count = spSubjectAnswers.Children.Count;
            for (int i = count - 1; i >= 0; i--)
            {
                spSubjectAnswers.Children.RemoveAt(i);
            }

            count = spObjectiveSideAnswers.Children.Count;
            for (int i = count - 1; i >= 0; i--)
            {
                spObjectiveSideAnswers.Children.RemoveAt(i);
            }

            count = spSubjectiveSideAnswers.Children.Count;
            for (int i = count - 1; i >= 0; i--)
            {
                spSubjectiveSideAnswers.Children.RemoveAt(i);
            }

            tbQuestion.Text = "";
        }

        private void SaveValues()
        {
            currentQuestionAnswers = new Question(questions[currentQuestion]);
            foreach (var item in spObjectAnswers.Children)
            {
                SaveObjectValues(item);
            }

            foreach (var item in spSubjectAnswers.Children)
            {
                SaveObjectValues(item);
            }

            foreach (var item in spObjectiveSideAnswers.Children)
            {
                SaveObjectValues(item);
            }

            foreach (var item in spSubjectiveSideAnswers.Children)
            {
                SaveObjectValues(item);
            }
            currentQuestionAnswers = null;
        }

        private void SaveObjectValues(object item)
        {
            StackPanel spAnswer = item as StackPanel;
            int cnt = spAnswer.Children.Count;
            bool check = false;
            foreach (var itemBox in spAnswer.Children)
            {
                if (itemBox is CheckBox)
                {
                    check = ((itemBox as CheckBox).IsChecked == true ? true : false);
                }
                else if (itemBox is TextBlock)
                {
                    String textOfAnswer = (itemBox as TextBlock).Text;
                    int index = currentQuestionAnswers.GetIndexAnswer(textOfAnswer);
                    wasAnswerAndHow[currentQuestion].Item2[index] = check;
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
                        resultTest[currentQuestion] = false;
                        throw new Exception("Неверный ответ!");
                    }
                }
                resultTest[currentQuestion] = true;
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        private void ShowNote2(object sender, RoutedEventArgs e)
        {
            WInforming wInformation = new WInforming();
            wInformation.Owner = this;
            wInformation.SaveInformation(IsRightAnswer(), questions[currentQuestion].GetNote2(), true);
            wInformation.ShowDialog();

            if ((sender as ToggleButton) != null)
            {
                (sender as ToggleButton).IsChecked = false;
            }
        }

        private void Closing_Window(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            //bool AllAnswers = true;
            //foreach (var item in wasAnswerAndHow)
            //{
            //    if (item.Item1[0] == false)
            //    {

            //        AllAnswers = false;
            //        break;
            //    }
            //}

            //if (AllAnswers)
            if (CheckAllAnswer())
            {
                this.closing_now = true;
                if (!finish)
                {
                    Finish(sender, null);
                }
                e.Cancel = false;
            }
            else
            {
                var result = MessageBox.Show("Вы действительно хотете выйти? \nТолько кнопка \"Завершить\" сохранит все данные!", "Подтверждение выхода", MessageBoxButton.OKCancel, MessageBoxImage.Question);
                
                if (result == MessageBoxResult.OK)
                {
                    this.Owner.Show();
                    e.Cancel = false;
                }
            }
            
        }

        public void SetUserName(string name)
        {
            UserName = name;
        }

        private void Window_ContentRendered_1(object sender, EventArgs e)
        {
            ShowQuestion();
            begin = DateTime.Now;
        }

        private void openFile2(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start(questions[currentQuestion].GetPathToFile2());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось октрыть файл");
            }
            if ((sender as ToggleButton) != null)
            {
                (sender as ToggleButton).IsChecked = false;
            }
        }
    }
}
