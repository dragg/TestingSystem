using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Application
{
    public class Answer
    {
        public String Text { get; set; }
        
        public bool Right { get; set; }

        public Answer()
        {
            Text = "";
            Right = false;
        }

        public Answer(String text, bool right = false)
        {
            ChangeAnswer(text, right);
        }

        public void ChangeAnswer(String text, bool right)
        {
            Text = text;
            Right = right;
        }

        public override string ToString()
        {
            return Text + (Right ? " - верный." : " - неверный.");
        }

        public void WriteAnswer(StreamWriter writer)
        {
            writer.WriteLine(Text + '\n' + Helper.Separation + '\n' + Right);
        }

        public void ReadAnswer(StreamReader read)
        {
            String answer = "", right = "", tempString = "";
            bool first = true;
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

            Text = answer;
            Right = Boolean.Parse(right);
            //String[] answer = read.ReadLine().Split(new Char[] {Helper.Separation});
            //Text = answer[0];
            //Right = Boolean.Parse(answer[1]);
        }
    }
}
