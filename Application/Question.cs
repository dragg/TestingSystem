using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Application
{
    public class Question
    {
        private String Text;
        private String Note;
        List<Answer> answers;
        private bool NewQuestion = true;

        public Question()
        {
            answers = new List<Answer>();
        }

        public Question(String text, String note, List<Answer> answers = null, bool newQuestion = true)
        {
            NewQuestion = newQuestion;
            Text = text;
            Note = note;
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

        public void WriteQuestion(StreamWriter write)
        {
            write.WriteLine(Text + Helper.Separation + Note);
            write.WriteLine(answers.Count);
            foreach (var answer in answers)
            {
                answer.WriteAnswer(write);
            }
        }

        public void ReadQuestion(StreamReader read)
        {
            String[] TextAndNote = read.ReadLine().Split(new Char[] {Helper.Separation});
            Text = TextAndNote[0];
            Note = TextAndNote[1];
            String cntString = read.ReadLine();
            int cnt = Int32.Parse(cntString);
            for (int i = 0; i < cnt; i++)
            {
                Answer answer = new Answer();
                answer.ReadAnswer(read);
                answers.Add(answer);
            }
            NewQuestion = false;
        }

        public bool IsNewQuestion()
        {
            return NewQuestion;
        }
    }
}
