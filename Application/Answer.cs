using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Application
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
            writer.WriteLine(Text + '\n' + Helper.Separation + '\n' + Right + '\n' + subjects[Subject]);
        }

        public void ReadAnswer(StreamReader read)
        {
            String answer = "", right = "", tempString = "";
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
                    answer += tempString + '\n';
                }

                if (tempString != "")
                {
                    //answer += '\n';
                }
                tempString = read.ReadLine();
            } while (tempString != Helper.Separation);
            answer = answer.Remove(answer.Length - 1);

            right = read.ReadLine();

            subject = Int32.Parse(read.ReadLine());

            Text = answer;
            Right = Boolean.Parse(right);
            Subject = subjects.ElementAt(subject - 1).Key;
            //String[] answer = read.ReadLine().Split(new Char[] {Helper.Separation});
            //Text = answer[0];
            //Right = Boolean.Parse(answer[1]);
        }
    }
}
