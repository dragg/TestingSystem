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
using CommonLibrary;
using Microsoft.Win32;
using System.IO;
using System.Windows.Controls.Primitives;

namespace Settings
{
    /// <summary>
    /// Interaction logic for Question.xaml
    /// </summary>
    public partial class WQuestion : Window
    {
        Question question = null;

        private String PathToFile = "",
            PathToFile2 = "";

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
            tbNote2.Text = q.GetNote2();

            PathToFile = q.GetPathToFile();
            tbPathToFile.Text = System.IO.Path.GetFileName(PathToFile);
            PathToFile2 = q.GetPathToFile2();
            tbPathToFile2.Text = System.IO.Path.GetFileName(PathToFile2);
            foreach (var answer in q.GetAllAnswer())
            {
                lbAnswers.Items.Add(answer);
            }
            //SortListBox();
        }

        private void AddAnswer(object sender, RoutedEventArgs e)
        {
            if (tempAnswer.Text.ToString() != "")
            {
                lbAnswers.Items.Add(new Answer(tempAnswer.Text.ToString(), 
                    (right.IsChecked != null ? (right.IsChecked == true ? true : false) : false),
                    cmbSubject.SelectionBoxItem as String));
                tempAnswer.Clear();
                right.IsChecked = false;
                //SortListBox();
            }
            else
            {
                MessageBox.Show("Поле ввода ответа - пусто");
            }
            if ((sender as ToggleButton) != null)
            {
                (sender as ToggleButton).IsChecked = false;
            }
        }

        private void SortListBox()
        {
            lbAnswers.Items.SortDescriptions.Clear();
            lbAnswers.Items.SortDescriptions.Add(new System.ComponentModel.SortDescription("Subject", System.ComponentModel.ListSortDirection.Ascending));
            lbAnswers.Items.SortDescriptions.Add(new System.ComponentModel.SortDescription("Right", System.ComponentModel.ListSortDirection.Descending));
        }

        private void DeleteAnswer(object sender, RoutedEventArgs e)
        {
            if (lbAnswers.SelectedIndex != -1)
            {
                lbAnswers.Items.RemoveAt(lbAnswers.SelectedIndex);
            }
            if ((sender as ToggleButton) != null)
            {
                (sender as ToggleButton).IsChecked = false;
            }
        }

        private void ChangeAnswer(object sender, RoutedEventArgs e)
        {
            if (lbAnswers.SelectedIndex != -1)
            {
                tempAnswer.Text = (lbAnswers.Items[lbAnswers.SelectedIndex] as Answer).Text;
                right.IsChecked = (lbAnswers.Items[lbAnswers.SelectedIndex] as Answer).Right;
                cmbSubject.SelectedValue = (lbAnswers.Items[lbAnswers.SelectedIndex] as Answer).Subject;
                lbAnswers.Items.RemoveAt(lbAnswers.SelectedIndex);
            }
            if ((sender as ToggleButton) != null)
            {
                (sender as ToggleButton).IsChecked = false;
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

        private bool SaveQuestion(Question q = null)
        {
            bool response = true;

            if (q == null)
            {
                List<Answer> answers = new List<Answer>();
                foreach (var answer in lbAnswers.Items)
                {
                    answers.Add(answer as Answer);
                }
                Question question = new Question(tbQuestion.Text,
                    tbNote.Text.ToString(), 
                    tbNote2.Text.ToString(),
                    PathToFile,
                    PathToFile2,
                    answers,
                    ((this.question == null) ? true : false));
                if (question.isValid())
                {
                    if (question.IsNewQuestion())
                    {
                        (this.Owner as WTeacher).AddQuestion(question);
                    }
                    else
                    {
                        (this.Owner as WTeacher).ChangeQuestion(question);
                    }
                    
                }
                else
                {
                    response = false;
                }
                
            }
            else
            {
                if (q.IsNewQuestion())
                {
                    (this.Owner as WTeacher).AddQuestion(q);
                }
                else
                {
                    (this.Owner as WTeacher).ChangeQuestion(question);
                }
            }
            
            //(Parent as WTeacher)
            save = true;

            return response;
        }

        private void SaveAndClose(object sender, RoutedEventArgs e)
        {
            if (tbQuestion.Text.ToString() == "" || 
                tbNote.Text.ToString() == "" || 
                lbAnswers.Items.Count == 0 || 
                tbNote2.Text.ToString() == "" ||
                PathToFile == "")
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
                //Save
                if (SaveQuestion())
                {
                    //Close
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Вопрос должен содержать один верный ответ в полях Объект, Субъект, Субъективная сторона и Объективная сторона!");
                }
            }
            if ((sender as ToggleButton) != null)
            {
                (sender as ToggleButton).IsChecked = false;
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

        private void Select_file(object sender, RoutedEventArgs e)
        {
            OpenFileDialog myDialog = new OpenFileDialog();
            myDialog.Filter = "Документ(*.doc;*.docx)|*.doc;*.docx";
            myDialog.CheckFileExists = true;
            if (myDialog.ShowDialog() == true)
            {
                PathToFile = myDialog.FileName;
                tbPathToFile.Text = myDialog.SafeFileName;
            }
            if ((sender as ToggleButton) != null)
            {
                (sender as ToggleButton).IsChecked = false;
            }
        }

        private void selectFile2(object sender, RoutedEventArgs e)
        {
            OpenFileDialog myDialog = new OpenFileDialog();
            myDialog.Filter = "Документ(*.doc;*.docx)|*.doc;*.docx";
            myDialog.CheckFileExists = true;
            if (myDialog.ShowDialog() == true)
            {
                PathToFile2 = myDialog.FileName;
                tbPathToFile2.Text = myDialog.SafeFileName;
            }
            if ((sender as ToggleButton) != null)
            {
                (sender as ToggleButton).IsChecked = false;
            }
        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            double w = SystemParameters.VirtualScreenWidth / 3 - 25;
            tbQuestion.Width = w;
            tbNote.Width = w;
            tbNote2.Width = w;

            w = SystemParameters.VirtualScreenWidth / 2 - 25;

            tempAnswer.Width = w;
            //double w = this.Width;
            //MessageBox.Show(w.ToString());
        }

        private void Window_SizeChanged_1(object sender, SizeChangedEventArgs e)
        {
        }

        
    }
}
