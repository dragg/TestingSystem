using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace CommonLibrary
{
    public class Question
    {
        private String Text;
        private String Note;
        private String Note2;
        private String PathToFile;
        private String PathToFile2;
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

        public Question(String text, String note, String note2, String pathToFile, String pathToFile2, List<Answer> answers = null, bool newQuestion = true)
        {
            NewQuestion = newQuestion;
            Text = text;
            Note = note;
            Note2 = note2;
            //PathToFile = pathToFile;
            PathToFile = GetRelativePath(Directory.GetCurrentDirectory(), pathToFile);
            //PathToFile2 = pathToFile2;
            PathToFile2 = GetRelativePath(Directory.GetCurrentDirectory(), pathToFile2);
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
            return System.IO.Path.GetFullPath(PathToFile);
        }

        public String GetPathToFile2()
        {
            return System.IO.Path.GetFullPath(PathToFile2);
        }

        public bool CheckFile()
        {
            return File.Exists(PathToFile);
        }

        public void CopyFileTo(String path)
        {
            File.Copy(PathToFile, path, true);
        }

        public void WriteQuestion(StreamWriter write)
        {
            write.WriteLine(Crypting.Encrypt(Text, Helper.Key));
            write.WriteLine("\n");
            write.WriteLine(Helper.Separation);
            write.WriteLine(Crypting.Encrypt(Note, Helper.Key));
            write.WriteLine("\n");
            write.WriteLine(Helper.Separation);
            write.WriteLine(Crypting.Encrypt(Note2, Helper.Key));
            write.WriteLine("\n");
            write.WriteLine(Helper.Separation);
            write.WriteLine(Crypting.Encrypt(PathToFile, Helper.Key));
            write.WriteLine("\n");
            write.WriteLine(Helper.Separation);
            write.WriteLine(Crypting.Encrypt(PathToFile2, Helper.Key));
            write.WriteLine("\n");
            write.WriteLine(Helper.Separation);

            write.WriteLine(Crypting.Encrypt(answers.Count.ToString(), Helper.Key));
            foreach (var answer in answers)
            {
                answer.WriteAnswer(write);
            }
        }

        public void ReadQuestion(StreamReader read)
        {
            try
            {
                String TextString = "",
                NoteString = "",
                Note2String = "",
                PathToFileString = "",
                PathToFileString2 = "",
                tempString = "",
                temp = "";
                bool first = true;
                do
                {
                    if (first)
                    {
                        first = !first;
                    }
                    else
                    {
                        if (temp != "")
                        {
                            TextString += tempString + '\n';
                        }
                    }

                    if (tempString != "")
                    {
                        //TextString += '\n';
                    }
                    temp = read.ReadLine();
                    tempString = Crypting.Decrypt(temp, Helper.Key);
                } while (temp != Helper.Separation);
                TextString = TextString.Remove(TextString.Length - 1);

                temp = tempString = "";
                first = true;
                do
                {
                    if (first)
                    {
                        first = !first;
                    }
                    else
                    {
                        if (temp != "")
                        {
                            NoteString += tempString + '\n';
                        }
                    }

                    if (tempString != "")
                    {
                        //NoteString += '\n';
                    }
                    temp = read.ReadLine();
                    tempString = Crypting.Decrypt(temp, Helper.Key);
                } while (temp != Helper.Separation);
                NoteString = NoteString.Remove(NoteString.Length - 1);

                temp = tempString = "";
                first = true;
                do
                {
                    if (first)
                    {
                        first = !first;
                    }
                    else
                    {
                        if (temp != "")
                        {
                            Note2String += tempString + '\n';
                        }
                    }

                    if (tempString != "")
                    {
                        //NoteString += '\n';
                    }
                    temp = read.ReadLine();
                    tempString = Crypting.Decrypt(temp, Helper.Key);
                } while (temp != Helper.Separation);
                Note2String = Note2String.Remove(Note2String.Length - 1);

                temp = tempString = "";
                first = true;
                do
                {
                    if (first)
                    {
                        first = !first;
                    }
                    else
                    {
                        if (temp != "")
                        {
                            PathToFileString += tempString + '\n';
                        }
                    }

                    if (tempString != "")
                    {
                        //NoteString += '\n';
                    }
                    temp = read.ReadLine();
                    tempString = Crypting.Decrypt(temp, Helper.Key);
                } while (temp != Helper.Separation);
                PathToFileString = PathToFileString.Remove(PathToFileString.Length - 1);

                temp = tempString = "";
                first = true;
                do
                {
                    if (first)
                    {
                        first = !first;
                    }
                    else
                    {
                        if (temp != "")
                        {
                            PathToFileString2 += tempString + '\n';
                        }
                    }

                    if (tempString != "")
                    {
                        //NoteString += '\n';
                    }
                    temp = read.ReadLine();
                    tempString = Crypting.Decrypt(temp, Helper.Key);
                } while (temp != Helper.Separation);
                PathToFileString2 = PathToFileString2.Remove(PathToFileString2.Length - 1);

                Text = TextString;
                Note = NoteString;
                Note2 = Note2String;
                PathToFile = PathToFileString;
                PathToFile2 = PathToFileString2;
                //String[] TextAndNote = read.ReadLine().Split(new Char[] {Helper.Separation});
                //Text = TextAndNote[0];
                //Note = TextAndNote[1];
                String cntString = tempString = Crypting.Decrypt(read.ReadLine(), Helper.Key);
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
            catch (Exception ex)
            {
                throw new Exception("Ошибка при чтении вопроса");
            }
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

        private static string GetRelativePath(string BasePath, string AbsolutePath)
        {
            char Separator = System.IO.Path.DirectorySeparatorChar;
            if (string.IsNullOrWhiteSpace(BasePath)) BasePath = Directory.GetCurrentDirectory();
            var ReturnPath = "";
            var CommonPart = "";
            var BasePathFolders = BasePath.Split(Separator);
            var AbsolutePathFolders = AbsolutePath.Split(Separator);
            var i = 0;
            while (i < BasePathFolders.Length & i < AbsolutePathFolders.Length)
            {
                if (BasePathFolders[i].ToLower() == AbsolutePathFolders[i].ToLower())
                {
                    CommonPart += BasePathFolders[i] + Separator;
                }
                else
                {
                    break;
                }
                i += 1;
            }
            if (CommonPart.Length > 0)
            {
                var parents = BasePath.Substring(CommonPart.Length - 1).Split(Separator);
                foreach (var ParentDir in parents)
                {
                    if (!string.IsNullOrEmpty(ParentDir))
                        ReturnPath += ".." + Separator;
                }
            }
            ReturnPath += AbsolutePath.Substring(CommonPart.Length);
            return ReturnPath;
        }
    }
}
