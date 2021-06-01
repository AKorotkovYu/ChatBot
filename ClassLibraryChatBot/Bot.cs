using System;
using System.Linq;
using System.Collections.Generic;
using System.Xml;
using System.IO;

namespace ClassLibraryChatBot
{
    public class Bot
    {
        public static string botName = "Шарпик"; 
        private readonly bool isFinal = false;

        public bool IsFinal
        {
            get { return isFinal; }
        }

        Random randomizer = new Random();

        public delegate string CommandMessage();

        public readonly List<Answer> Jokes = new List<Answer>();
        public readonly List<Answer> Aphorisms = new List<Answer>();
        public readonly List<Answer> Meetings = new List<Answer>();
        public readonly List<Answer> InitPhrases = new List<Answer>();

        readonly Dictionary<Question, CommandMessage> Commands = new Dictionary<Question, CommandMessage>();
        readonly List<Question> Questions = new List<Question>();
        static readonly List<String> Phrases = new List<String>();

        public Bot(List<Question> QuestionsList, List<Answer> AnswersList, Dictionary<Question, CommandMessage> Commands)
        {
            Split(AnswersList);
            Questions = QuestionsList;
            this.Commands = Commands;
        }

        /// <summary>
        /// разбиение входных данных на листы  
        /// </summary>
        /// <param name="AnswersList">Входные данные</param>
        private void Split(List<Answer> AnswersList)
        {
            foreach(var Answer in AnswersList)
            {
                switch(Answer.AnswerType)
                {
                    case "jok":
                            Jokes.Add(Answer);
                        break;
                    case "aph":
                            Aphorisms.Add(Answer);
                        break;
                    case "met":
                            Meetings.Add(Answer);
                        break;
                    case "ini":
                            InitPhrases.Add(Answer);
                        break;
                }
            }
        }


        /// <summary>
        /// Главный метод, принимающий вопрос пользователя и выдающиий подходящий ответ из базы
        /// </summary>
        /// <param name="asking">Строка-вопрос, вводимая пользователем</param>
        /// <returns></returns>
        public string TakeAnswer(string asking)
        {
            asking = asking.ToLower();
            string answer_line = String.Empty;
            string secondpart = String.Empty;

            Phrases.Clear();

            foreach (var command in Commands)
            if (command.Key.Phrase == asking)
            {
                return command.Value();
            }

            if (asking=="анекдот")
            {
                return TakeRandAnswer(Jokes);
            }

            foreach (Question oneQuestion in Questions)
            {
                if (oneQuestion.Phrase == asking)
                {
                    foreach (Answer an in Meetings)
                    {
                        foreach (int id in an.questionIDs)
                        {
                            if (oneQuestion.ID == id)
                            {
                                Phrases.Add(an.Phrase);
                            }
                        }
                    }

                    var va = TakeRandFromList(Phrases);
                    return va;
                }
            }
   
            answer_line = TakeRandAnswer(InitPhrases);
            if(answer_line!="ERROR")  
                secondpart = TakeRandAnswer(Aphorisms);
            answer_line += " ";
            answer_line += secondpart;
            return answer_line;
        }

        /// <summary>
        /// выбрать случайный текстовый ответ из выборки
        /// </summary>
        /// <param name="lPhrases">Лист строк из которых идёт выборка</param>
        /// <returns></returns>
        string TakeRandFromList(List<String> lPhrases)
        {
            int r = randomizer.Next(0, lPhrases.Count());
            var an = lPhrases.Skip(r).Take(1).ToArray();
            return an[0];
        }

        /// <summary>
        /// выбрать случайный ответ из всей переданной базы
        /// </summary>
        /// <param name="lAnswer">Лист объектов-ответов из которого идёт выборка ответов</param>
        /// <returns></returns>
        string TakeRandAnswer(List<Answer> lAnswer)
        {
            if (lAnswer.Count == 0)
                return "ERROR";
            int r = randomizer.Next(0, lAnswer.Count());
            var an1 = lAnswer.Skip(r).Take(1).ToArray();
            
            return an1[0].Phrase;
        }
    }
}
