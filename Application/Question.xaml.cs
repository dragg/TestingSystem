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

namespace Application
{
    /// <summary>
    /// Interaction logic for Question.xaml
    /// </summary>
    public partial class WQuestion : Window
    {
        Question question = null;

        private bool save = false;

        public WQuestion()
        {
            InitializeComponent();
        }

        public WQuestion(Question q)
        {
            question = q;
            InitializeComponent();
            tbQuestion.Text = q.GetQuestion();
            tbNote.Text = q.GetNote();
            foreach (var answer in q.GetAllAnswer())
            {
                lbAnswers.Items.Add(answer);
            }
        }

        private void AddAnswer(object sender, RoutedEventArgs e)
        {
            if (tempAnswer.Text.ToString() != "")
            {
                lbAnswers.Items.Add(new Answer(tempAnswer.Text.ToString(), (right.IsChecked != null ? (right.IsChecked == true ? true : false) : false)));
                tempAnswer.Clear();
                right.IsChecked = false;
            }
            else
            {
                MessageBox.Show("Поле ввода ответа - пусто");
            }
        }

        private void DeleteAnswer(object sender, RoutedEventArgs e)
        {
            if (lbAnswers.SelectedIndex != -1)
            {
                lbAnswers.Items.RemoveAt(lbAnswers.SelectedIndex);
            }
        }

        private void ChangeAnswer(object sender, RoutedEventArgs e)
        {
            if (lbAnswers.SelectedIndex != -1)
            {
                tempAnswer.Text = (lbAnswers.Items[lbAnswers.SelectedIndex] as Answer).Text;
                right.IsChecked = (lbAnswers.Items[lbAnswers.SelectedIndex] as Answer).Right;
                lbAnswers.Items.RemoveAt(lbAnswers.SelectedIndex);
            }
        }

        private void listAnswers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lbAnswers.SelectedIndex != -1)
            {
                btChangeAnswer.IsEnabled = btDeleteAnswer.IsEnabled = true;
            }
            else
            {
                btChangeAnswer.IsEnabled = btDeleteAnswer.IsEnabled = false;
            }
        }

        private void SaveQuestion(Question q = null)
        {
            if (q == null)
            {
                List<Answer> answers = new List<Answer>();
                foreach (var answer in lbAnswers.Items)
                {
                    answers.Add(answer as Answer);
                }
                (this.Owner as WTeacher).AddQuestion(new Question(tbQuestion.Text, tbNote.Text.ToString(), answers, false));
            }
            else
            {
                (this.Owner as WTeacher).AddQuestion(q);
            }
            
            //(Parent as WTeacher)
            save = true;
        }

        private void SaveAndClose(object sender, RoutedEventArgs e)
        {
            if (tbQuestion.Text.ToString() == "" || tbNote.Text.ToString() == "" || lbAnswers.Items.Count == 0)
            {
                MessageBox.Show("Пожалуйста, заполните полностью вопрос или выйдите без сохранения!");
            }
            else
            {
                //Проверка на хоть один верный ответ
                bool one = false;
                foreach (var answer in lbAnswers.Items)
                {
                    if ((answer as Answer).Right)
                    {
                        one = true;
                        break;
                    }
                }

                if (!one)
                {
                    MessageBox.Show("Нет ни одного верного ответа!");
                }
                else
                {
                    //Save
                    SaveQuestion();

                    //Close
                    this.Close();
                }
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!save)
            {
                NotSaverChange wNSC = new NotSaverChange();
                if (!(wNSC.ShowDialog() == true))
                {
                    e.Cancel = true;
                }
                else
                {
                    if (question != null && !question.IsNewQuestion())
                    {
                         SaveQuestion(question);
                    }
                }
            }
        }
    }
}
