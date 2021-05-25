using System;
using System.Collections.Generic;
using System.Text;

namespace ClassChatBot
{
    
    sealed class Question
    {
        private static int m_newId = 0;
        public readonly int ID;
        private string phrase;
        private string questionType;
        public string Phrase { get { return phrase; } }
        public string QuestionType { get { return questionType; } }

        static Question()
        {
            m_newId = 0;
        }

        private Question()
        {
            /*-------------------------------------*/
        }

        public Question(string phrase, string type_of_question = "ask") //ask or com
        {
            m_newId++;
            this.questionType = type_of_question;
            ID = m_newId;
            this.phrase = phrase;
        }
    }
}
