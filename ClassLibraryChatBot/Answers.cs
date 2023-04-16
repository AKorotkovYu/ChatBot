using System.Collections.Generic;

namespace ClassLibraryChat
{ 
    public sealed class Answer
    {
        private static int newId;
        internal readonly int ID;
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

        public Answer(List<int> questionIds, string phrase, string typeOfAnswer)
        {
            newId++;
            ID = newId;
            answerType = typeOfAnswer;
            questionIDs = questionIds;
            this.phrase = phrase;
        }

        public Answer(string Phrase, string typeOfAnswer)
        {
            List<int> questionIds = new List<int>() { -1 };
            newId++;
            ID = newId;
            answerType = typeOfAnswer;
            questionIDs = questionIds;
            phrase = Phrase;
        }

        public void AddQuestionID(int questionID)
        {
            questionIDs.Add(questionID);
        }
    }
}

