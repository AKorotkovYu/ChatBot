using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ClassLibraryChatBot
{
    public class Bot
    {
        public string botName = "Шарпик"; 
        private readonly bool isFinal = false;

        public bool IsFinal
        {
            get { return isFinal; }
        }

        readonly Random randomizer = new Random();

        public delegate string CommandMessage();
        readonly Dictionary<Question, CommandMessage> commands = new Dictionary<Question, CommandMessage>();
        public readonly List<Answer> Answers = new List<Answer>();
        readonly List<String> Phrases = new List<String>();


        public Bot(String botName, List<Answer> AnswersList, Dictionary<Question, CommandMessage> commands)
        {
            Answers = AnswersList;
            

            foreach(var command in commands)
                this.commands.Add(command.Key,command.Value);
            commands.Add(new Question("/как тебя зовут"), () => { return "Меня зовут:"; });
            this.botName = botName;
        }

        /// <summary>
        /// Главный метод, принимающий вопрос пользователя и выдающиий подходящий ответ из базы
        /// </summary>
        /// <param name="asking">Строка-вопрос, вводимая пользователем</param>
        /// <returns>Ответ бота</returns>
        public string TakeAnswer(string asking)
        {
            //Thread.Sleep(5000);
            asking = asking.ToLower();
            string answer_line = String.Empty;
            string secondpart = String.Empty;

            Phrases.Clear();

            string answer;
                foreach (var command in commands)
                   if(command.Key.Phrase==asking)
                    {
                        if(command.Value()=="Меня зовут:")
                        {
                            answer=command.Value + " " + botName;
                        }    
                        else 
                            answer= command.Value();
                        return answer;
                    }
            return null;
        }
    }
}
