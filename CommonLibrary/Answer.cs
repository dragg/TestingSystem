using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace CommonLibrary
{
    public class Answer
    {
        private Dictionary<String, int> subjects = new Dictionary<String, int>()
        {
            {"Объект", 1},
            {"Субъект", 2},
            {"Субъектная сторона", 3},
            {"Объктная сторона", 4}
        };

        public String Text { get; set; }
        
        public bool Right { get; set; }

        public String Subject { get; set; }

        public Answer()
        {
            Text = "";
            Right = false;
            Subject = "";
        }

        public Answer(String text, bool right = false, String subject = "")
        {
            ChangeAnswer(text, right, subject);
        }

        public void ChangeAnswer(String text, bool right, String subject)
        {
            Text = text;
            Right = right;
            Subject = subject;
        }

        public bool isRight()
        {
            return Right;
        }

        public override string ToString()
        {
            return Subject + (Right ? " - верный" : " - неверный") + " - " + Text;
        }

        public void WriteAnswer(StreamWriter writer)
        {
            writer.WriteLine(Crypting.Encrypt(Text, Helper.Key));
            writer.WriteLine("\n");
            writer.WriteLine(Helper.Separation);
            writer.WriteLine("\n");
            writer.WriteLine(Crypting.Encrypt(Right.ToString(), Helper.Key));
            writer.WriteLine("\n");
            writer.WriteLine(Crypting.Encrypt(subjects[Subject].ToString(), Helper.Key));
        }

        public void ReadAnswer(StreamReader read)
        {
            String answer = "", right = "", tempString = "",
                temp = "";
            bool first = true;
            int subject = 0;
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
                        answer += tempString + '\n';
                    }
                }

                if (tempString != "")
                {
                    //answer += '\n';
                }
                temp = read.ReadLine();
                tempString = Crypting.Decrypt(temp, Helper.Key);
            } while (temp != Helper.Separation);
            answer = answer.Remove(answer.Length - 1);

            temp = "";
            while (temp == "")
            {
                temp = read.ReadLine();
            }
            right = Crypting.Decrypt(temp, Helper.Key);

            temp = "";
            while (temp == "")
            {
                temp = read.ReadLine();
            }
            subject = Int32.Parse(Crypting.Decrypt(temp, Helper.Key));

            Text = answer;
            Right = Boolean.Parse(right);
            Subject = subjects.ElementAt(subject - 1).Key;
            //String[] answer = read.ReadLine().Split(new Char[] {Helper.Separation});
            //Text = answer[0];
            //Right = Boolean.Parse(answer[1]);
        }
    }
}
