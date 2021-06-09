using System;
using System.Linq;
using System.Xml;
using ClassLibraryChatBot;
using System.Collections.Generic;

namespace ConsoleChatBot
{
    class Program  
    {
        static Random randomizer = new Random();
        static List<Bot> bots = new List<Bot>();

        static void Main()
        { 
            Dictionary<Question, Bot.CommandMessage> commands = new Dictionary<Question, Bot.CommandMessage>();
            List<Answer> jokes = new List<Answer>();
            List<Answer> meetings = new List<Answer>();
            List<Question> questions = new List<Question>();
            List<Answer> answers = new List<Answer>();

            List<Chat> allChats = new List<Chat>();

            string ask = String.Empty;
            bool isFinal = true;

            GetAnswersBase(meetings, "../../../XML/", "met");
            GetAnswersBase(jokes, "../../../XML/", "jok");

            //Комманды на вход боту
            commands.Add(new Question("привет"), () => { return TakeRandAnswer(meetings); });
            commands.Add(new Question("анекдот"), () => { return TakeRandAnswer(jokes); });

            bots.Add(new Bot("sillyBot", answers, commands));
            
            commands.Add(new Question("current"), () => { return DateTime.Now.ToString(); });
            commands.Add(new Question("через"), () => { return DateTime.Now.ToString().Split(" ").ToArray()[0]; });
            commands.Add(new Question("сколько времени"), () => { return DateTime.Now.ToString().Split(" ").ToArray()[0]; });
            commands.Add(new Question("брось кубик"), () => { return randomizer.Next(0, 6).ToString(); });
            commands.Add(new Question("подбрсь монетку"), () => { return (randomizer.Next(0, 6) == 1) ? "Орёл" : "Режка"; });
            commands.Add(new Question("до свидания"), () => { return "До cвидания"; });
            commands.Add(new Question("пока"), () => { return "До cвидания"; });            
            
            bots.Add(new Bot("smartbot", answers, commands));

            string usersFolder = "../../../Binary";
            string historyFolder = "../../../Binary";
            
            do
            {
                System.Console.WriteLine("enter 'start-chat'");
                ask = Console.ReadLine();
                isFinal = false;
            } 
            while (ask != "start-chat");

            Chat onechat = new Chat("chat", bots, usersFolder, historyFolder);
            
            do
            {
                ask = Console.ReadLine();

                switch (ask.Split(" ")[0])
                {
                    case "sign"://добавление пользователя
                        {
                            if(onechat.SignUser(ask))
                            {
                                Console.WriteLine("Пользователь вошёл в чат");
                            }
                            else
                            {
                                Console.WriteLine("Пользователь не добавлен");
                            }
                        }
                        break;

                    case "logout"://выход пользователя и удаление его из базы залогинившихся
                        {
                            if (onechat.LogOut(ask))
                            {
                                Console.WriteLine("Пользователь покинул чат");
                            }
                            else
                            {
                                Console.WriteLine("Пользователь покинул чат");
                            }
                        } 
                        break;

                    case "add-mes"://добавление сообщения 
                        {
                            if(onechat.AddMes(ask))
                            {
                                Console.WriteLine(ask);
                            }
                            else
                            {
                                Console.WriteLine("Сообщение не отправлено"); 
                            }
                        }
                        break;
                    case "view-his"://просмотреть все сообщения в историия
                        {
                            var history = onechat.GetHistory();
                            foreach(var message in history)
                            {
                                Console.WriteLine(message.ID+" "+message.DateTime+" "+message.Nickname+" "+message.Message);
                            }
                        }
                        break;
                    case "del-mes"://удаление сообщения
                        {
                            if(onechat.DelMes(ask))
                            {
                                Console.WriteLine("Сообщение удалено");
                            }
                            else
                            {
                                Console.WriteLine("Сообщение не найдено или написано не сегодня");
                            }
                        }
                        break;

                    case "bot"://обращение к боту
                        {
                            Console.WriteLine(onechat.BotCommand(ask));
                        }
                        break;

                    case "stop-chat":
                        {
                            isFinal = onechat.StopChat();
                            Console.WriteLine("Чат закончен");
                        }
                        break;
                }
                
                onechat.RefillUsersBase(usersFolder);
                onechat.RefillHistoryBase(historyFolder);
            }
            while (!isFinal);
        }

        /// <summary>
        /// Выдать случайный ответ из предложенных
        /// </summary>
        /// <param name="lAnswer">лист с ответами на вход</param>
        /// <returns></returns>
        static string TakeRandAnswer(List<Answer> lAnswer)
        {
            if (lAnswer.Count == 0)
                return "ERROR";
            int r = randomizer.Next(0, lAnswer.Count());
            var an1 = lAnswer.Skip(r).Take(1).ToArray();

            return an1.FirstOrDefault()?.Phrase;
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


        /// <summary>
        /// сохранение фразы любого типа в файл
        /// </summary>
        /// <param name="fileName">Имя-тип XML файла</param>
        /// <param name="text">Добавляемый</param>
        public void putAnswersBase(string fileName, string text)
        {
            var xDoc = new XmlDocument();
            string pathXMLFolder = "../../../XML/";
            xDoc.Load(pathXMLFolder + fileName + ".xml");

            XmlElement xRoot = xDoc.DocumentElement;
            XmlElement typeElem = xDoc.CreateElement(fileName);
            XmlAttribute nameAttr = xDoc.CreateAttribute("text");
            XmlText nameText = xDoc.CreateTextNode(text);

            nameAttr.AppendChild(nameText);
            typeElem.Attributes.Append(nameAttr);
            xRoot.AppendChild(typeElem);
            xDoc.Save(pathXMLFolder + fileName + ".xml");
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
            string pathXMLFolder = "../../../XML/";
            xDoc.Load(pathXMLFolder + fileName + ".xml");

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
            xDoc.Save(pathXMLFolder + fileName + ".xml");
        }
    }
}
