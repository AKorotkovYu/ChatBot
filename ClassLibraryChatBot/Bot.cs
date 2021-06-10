using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;


namespace ClassLibraryChat
{

    public class Bot: User
    {
        private readonly bool isFinal = false;

        public bool IsFinal
        {
            get { return isFinal; }
        }

        Dictionary<Question, Bot.CommandMessage> commands = new Dictionary<Question, Bot.CommandMessage>();
        List<Answer> jokes = new List<Answer>();
        List<Answer> meetings = new List<Answer>();
        

        public delegate string CommandMessage();

        Random randomizer = new Random();

        private readonly List<string> Phrases = new List<string>();
        string XMLFolder = String.Empty;

        public Bot(String botName, string historyPath, string XMLFolder):base(botName, historyPath)
        {
            this.XMLFolder = XMLFolder;
            GetAnswersBase(meetings, XMLFolder, "/met");
            GetAnswersBase(jokes, XMLFolder, "/jok");

            commands.Add(new Question("как тебя зовут"), () => { return "Меня зовут:"+botName; });
            commands.Add(new Question("current"), () => { return DateTime.Now.ToString(); });
            commands.Add(new Question("через"), () => { return DateTime.Now.ToString().Split(" ").ToArray()[0]; });
            commands.Add(new Question("сколько времени"), () => { return DateTime.Now.ToString().Split(" ").ToArray()[0]; });
            commands.Add(new Question("брось кубик"), () => { return randomizer.Next(0, 6).ToString(); });
            commands.Add(new Question("подбрсь монетку"), () => { return (randomizer.Next(0, 6) == 1) ? "Орёл" : "Режка"; });
            commands.Add(new Question("до свидания"), () => { return "До cвидания"; });
            commands.Add(new Question("пока"), () => { return "До cвидания"; });
            commands.Add(new Question("привет"), () => { return TakeRandAnswer(meetings); });
            commands.Add(new Question("анекдот"), () => { return TakeRandAnswer(jokes); });
        }

        /// <summary>
        /// Выдать случайный ответ из предложенных
        /// </summary>
        /// <param name="lAnswer">лист с ответами на вход</param>
        /// <returns></returns>
        string TakeRandAnswer(List<Answer> lAnswer)
        {
            if (lAnswer.Count == 0)
                return "ERROR";
            int r = randomizer.Next(0, lAnswer.Count());
            var an1 = lAnswer.Skip(r).Take(1).ToArray();

            return an1.FirstOrDefault()?.Phrase;
        }

        /// <summary>
        /// Главный метод, принимающий вопрос пользователя и выдающиий подходящий ответ из базы
        /// </summary>
        /// <param name="asking">Строка-вопрос, вводимая пользователем</param>
        /// <returns>Ответ бота</returns>
        public Message SendMessage(User user, string asking)
        {
            asking = asking.ToLower();
            string answer=String.Empty;
            Message message;
            Phrases.Clear();
            
            foreach (var command in commands)
                if(command.Key.Phrase==asking)
                {
                        message = new Message(DateTime.Now.ToString(), Nickname, command.Value());
                }
            
            if (answer != null)
            {
                messages.Add(new Message( DateTime.Now.ToString(), user.Nickname, asking));
                messages.Add(new Message( DateTime.Now.ToString(), Nickname, answer));
                
            }
            return messages.Last();
        }

        /// <summary>
        /// сохранение фразы любого типа в файл
        /// </summary>
        /// <param name="fileName">Имя-тип XML файла</param>
        /// <param name="text">Добавляемый</param>
        public void putAnswersBase(string fileName, string text)
        {
            var xDoc = new XmlDocument();
            xDoc.Load(XMLFolder + fileName + ".xml");

            XmlElement xRoot = xDoc.DocumentElement;
            XmlElement typeElem = xDoc.CreateElement(fileName);
            XmlAttribute nameAttr = xDoc.CreateAttribute("text");
            XmlText nameText = xDoc.CreateTextNode(text);

            nameAttr.AppendChild(nameText);
            typeElem.Attributes.Append(nameAttr);
            xRoot.AppendChild(typeElem);
            xDoc.Save(XMLFolder + fileName + ".xml");
        }


        /// <summary>
        /// сохранение любого вариантом соответствий ответов
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="text"></param>
        /// <param name="IDs"></param>
        public void putAnswersBase(string fileName, string text, string IDs)
        {
            var xDoc = new XmlDocument();
            xDoc.Load(XMLFolder + fileName + ".xml");

            XmlElement xRoot = xDoc.DocumentElement;
            XmlElement typeElem = xDoc.CreateElement(fileName);
            XmlAttribute nameAttr = xDoc.CreateAttribute("text");
            XmlAttribute IDAttr = xDoc.CreateAttribute("CorrectIDsAnswers");
            XmlText nameText = xDoc.CreateTextNode(text);
            XmlText nameIDs = xDoc.CreateTextNode(IDs);

            nameAttr.AppendChild(nameText);
            IDAttr.AppendChild(nameIDs);
            typeElem.Attributes.Append(nameAttr);
            typeElem.Attributes.Append(IDAttr);
            xRoot.AppendChild(typeElem);
            xDoc.Save(XMLFolder + fileName + ".xml");
        }

        /// <summary>
        /// получение всех ответов из базы
        /// </summary>
        /// <param name="questions">Лист объектов-вопросов</param>
        /// <param name="fileName">Имя файла</param>
        static void GetQuestionBase(List<Question> questions, string folderPath, String fileName)
        {
            Question qBuff;
            var xDoc = new XmlDocument();
            xDoc.Load(folderPath + fileName + ".xml");
            var xRoot = xDoc.DocumentElement;

            foreach (XmlNode xNode in xRoot)
            {
                if (xNode.Attributes.Count > 0)
                {
                    XmlNode attr = xNode.Attributes.GetNamedItem("text");
                    if (attr != null)
                    {
                        questions.Add(qBuff = new Question(attr.Value, fileName));
                    }
                }
            }
        }

        /// <summary>
        /// получение всех вопросов из базы
        /// </summary>
        /// <param name="answers">Лист объектов-ответов</param>
        /// <param name="fileName">Имя файла</param>
        static void GetAnswersBase(List<Answer> answers, string folderPath, String fileName)
        {
            Answer aBuff;
            var xDoc = new XmlDocument();
            xDoc.Load(folderPath + fileName + ".xml");
            XmlElement xRoot = xDoc.DocumentElement;

            foreach (XmlNode xNode in xRoot)
            {
                if (xNode.Attributes.Count > 0)
                {
                    if (fileName == "met")
                    {
                        XmlNode attrtext = xNode.Attributes.GetNamedItem("text");
                        if (attrtext != null)
                        {
                            XmlNode attrID = xNode.Attributes.GetNamedItem("CorrectIDsAnswers");
                            if (attrID != null)
                            {
                                string[] splitedAttr = attrID.Value.ToString().Split(" ");
                                List<int> questionIDs = new List<int>();
                                foreach (var id in splitedAttr)
                                {
                                    if (id != "")
                                        questionIDs.Add(Int32.Parse(id));
                                }
                                aBuff = new Answer(questionIDs, attrtext.Value, fileName);
                                answers.Add(aBuff);
                            }
                        }
                    }
                    else
                    {
                        if (xNode.Attributes.Count > 0)
                        {
                            XmlNode attr = xNode.Attributes.GetNamedItem("text");
                            if (attr != null)
                            {
                                aBuff = new Answer(attr.Value, fileName);
                                answers.Add(aBuff);
                            }
                        }
                    }
                }
            }
        }
    }
}
