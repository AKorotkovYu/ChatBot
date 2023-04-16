namespace ClassLibraryChat
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

        public Question(string phrase, string questionType = "ask")
        {
            newId++;
            this.questionType = questionType;
            ID = newId;
            this.phrase = phrase;
        }
    }
}
