using System;
using System.Collections.Generic;
using System.Text;

namespace ClassLibraryChatBot
{ 
    public sealed class Answer
    {
        private static int newId;
        protected readonly int ID;
        public readonly List<int> questionIDs;

        private readonly string answerType;
        public string AnswerType { get { return answerType; } }
        private readonly string phrase;
        public string Phrase { get { return phrase; } }

        static Answer()
        {
            newId = 0;
        }

        private Answer()
        {

        }

        public Answer(List<int> questionIds, string phrase, string type_of_answer)
        {
            newId++;
            ID = newId;
            this.answerType = type_of_answer;
            this.questionIDs = questionIds;
            this.phrase = phrase;
        }

        public Answer(string Phrase, string type_of_answer)
        {
            List<int> questionIds = new List<int>() { -1 };
            newId++;
            ID = newId;
            this.answerType = type_of_answer;
            this.questionIDs = questionIds;
            this.phrase = Phrase;
        }

        public void AddQuestionID(int questionID)
        {
            questionIDs.Add(questionID);
        }
    }
}

