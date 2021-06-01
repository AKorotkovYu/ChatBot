using System;
using System.Collections.Generic;
using System.Text;

namespace ClassLibraryChatBot
{
    public sealed class Question
    {
        private static int newId = 0;
        public readonly int ID;
        private readonly string phrase;
        private readonly string questionType;
        public string Phrase { get { return phrase; } }
        public string QuestionType { get { return questionType; } }

        static Question()
        {
            newId = 0;
        }

        public Question(string phrase, string type_of_question = "ask") //ask or com
        {
            newId++;
            this.questionType = type_of_question;
            ID = newId;
            this.phrase = phrase;
        }
    }
}
