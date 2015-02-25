using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace CommonLibrary
{
    public class Question
    {
        private String Text;
        private String Note;
        private String Note2;
        private String PathToFile;
        List<Answer> answers;
        private bool NewQuestion = true;

        public bool isValid()
        {
            bool response = true;

            List<Answer> rightAnswers = new List<Answer>();

            //Находим верные ответы
            foreach (var answer in answers)
            {
                if (answer.isRight())
                {
                    rightAnswers.Add(answer);
                }
            }

            //По каждому объекту(Объект, Субъект, ...) должно быть по одному ответы, поэтому всего 4
            if (rightAnswers.Count == 4)
            {
                String s1 = rightAnswers[0].Subject,
                    s2 = rightAnswers[1].Subject,
                    s3 = rightAnswers[2].Subject,
                    s4 = rightAnswers[3].Subject;

                //Проверяем, чтобы не повторялись по объекту
                if (!(s1 != s2 && s1 != s3 && s1 != s4 &&
                    s2 != s3 && s2 != s4 &&
                    s3 != s4))
                {
                    response = false;
                }
            }
            else
            {
                response = false;
            }

            return response;
        }

        public Question()
        {
            answers = new List<Answer>();
        }

        public Question(String text, String note, String note2, String pathToFile, List<Answer> answers = null, bool newQuestion = true)
        {
            NewQuestion = newQuestion;
            Text = text;
            Note = note;
            Note2 = note2;
            PathToFile = pathToFile;
            this.answers = (answers == null ? new List<Answer>() : new List<Answer>(answers));
        }

        public void AddAnswer(Answer answer)
        {
            answers.Add(answer);
        }

        public void DeleteAnswer(int index)
        {
            answers.RemoveAt(index);
        }

        public List<Answer> GetAllAnswer()
        {
            return new List<Answer>(answers);
        }

        public int GetIndexAnswer(String answer)
        {
            int i = 0;
            foreach (var item in answers)
            {
                if (item.Text == answer)
                {
                    break;
                }
                i++;
            }
            return i;
        }

        public override String ToString()
        {
            return Text;
        }

        public String GetQuestion()
        {
            return Text;
        }

        public String GetNote()
        {
            return Note;
        }

        public String GetNote2()
        {
            return Note2;
        }

        public String GetPathToFile()
        {
            return PathToFile;
        }

        public void WriteQuestion(StreamWriter write)
        {
            write.WriteLine(Text + '\n' + Helper.Separation + '\n' + 
                Note + '\n' + Helper.Separation + '\n' + 
                Note2 + '\n' + Helper.Separation + '\n' + 
                PathToFile);
            write.WriteLine(answers.Count);
            foreach (var answer in answers)
            {
                answer.WriteAnswer(write);
            }
        }

        public void ReadQuestion(StreamReader read)
        {
            String TextString = "", NoteString = "", tempString = "";
            bool first = true;
            do
            {
                if (first)
                {
                    first = !first;
                }
                else
                {
                    TextString += tempString + '\n';
                }
                
                if (tempString != "")
                {
                    //TextString += '\n';
                }
                tempString = read.ReadLine();
            } while (tempString != Helper.Separation);
            TextString = TextString.Remove(TextString.Length - 1);
            tempString = "";
            first = true;

            do
            {
                if (first)
                {
                    first = !first;
                }
                else
                {
                    NoteString += tempString + '\n';
                }

                if (tempString != "")
                {
                    //NoteString += '\n';
                }
                tempString = read.ReadLine();
            } while (tempString != Helper.Separation);
            NoteString = NoteString.Remove(NoteString.Length - 1);

            Text = TextString;
            Note = NoteString;
            //String[] TextAndNote = read.ReadLine().Split(new Char[] {Helper.Separation});
            //Text = TextAndNote[0];
            //Note = TextAndNote[1];
            String cntString = read.ReadLine();
            int cnt = Int32.Parse(cntString);
            for (int i = 0; i < cnt; i++)
            {
                Answer answer = new Answer();
                answer.ReadAnswer(read);
                answers.Add(answer);
            }
            NewQuestion = false;


            SortBySubject();
        }

        private void SortBySubject()
        {
            //Тут будем хранить отсортированные ответы
            List<Answer> tempAnswers = new List<Answer>();

            //Будем сортировать в этом порядке
            List<String> names = new List<string>()
            {
                "Объект",
                "Объктная сторона",
                "Субъект",
                "Субъектная сторона"
            };

            //Сортируем
            for (int i = 0; i < names.Count; i++)
            {
                for (int j = 0; j < answers.Count; j++)
                {
                    if (answers[j].Subject == names[i])
                    {
                        tempAnswers.Add(answers[j]);
                    }
                }
            }

            //Записываем результат сортировки
            answers = tempAnswers;
        }

        public bool IsNewQuestion()
        {
            return NewQuestion;
        }
    }
}
