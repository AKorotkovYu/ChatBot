using System;
using System.Xml;
using System.Linq;
using System.IO;
using ClassLibraryChatBot;
using System.Collections.Generic;

namespace ConsoleChatBot
{
    class Program
    {
        static List<(int ID, string dateTime, string nickname,  string message)> messagesTurples = new List<(int ID, string dateTime, string nickname, string message)>();
        static List<User> users = new List<User>();
        static Dictionary<Question, Bot.CommandMessage> commands = new Dictionary<Question, Bot.CommandMessage>();
        static Random randomizer = new Random();

        static string message=String.Empty;

        static int messageID = 0;

        static void Main()
        { 
            List<Answer> jokes = new List<Answer>();
            List<Answer> meetings = new List<Answer>();
            List<Question> questions = new List<Question>();
            List<Answer> answers = new List<Answer>();
            List<Bot> bots = new List<Bot>();

            string asking = String.Empty;
            bool isFinal = true;

            GetXML(meetings, "met");
            GetXML(jokes, "jok");

            //Комманды на вход боту
            commands.Add(new Question("привет"), () => { return TakeRandAnswer(meetings); });
            commands.Add(new Question("анекдот"), () => { return TakeRandAnswer(jokes); });
            commands.Add(new Question("current"), () => { return DateTime.Now.ToString(); });
            commands.Add(new Question("через"), () => { return DateTime.Now.ToString().Split(" ").ToArray()[0]; });
            commands.Add(new Question("сколько времени"), () => { return DateTime.Now.ToString().Split(" ").ToArray()[0]; });
            commands.Add(new Question("брось кубик"), () => { return randomizer.Next(0, 6).ToString(); });
            commands.Add(new Question("подбрсь монетку"), () => { return (randomizer.Next(0, 6) == 1) ? "Орёл" : "Режка"; });
            commands.Add(new Question("до свидания"), () => { return "До cвидания"; });
            commands.Add(new Question("пока"), () => { return "До cвидания"; });            
            
            bots.Add(new Bot("Шарпик", answers, commands));
            bots.Add(new Bot("Шарпик2", answers, commands));
            getUsersBase();
            getHistoryBase();

            string nickname = String.Empty;
            string botName = String.Empty;

            do
            {
                asking = Console.ReadLine();
                var splittedAsking = asking.Split().ToArray();

                switch (splittedAsking[0])
                {
                    case "start-chat"://начало чата
                        { 
                            isFinal = false;
                        }
                        break;

                    case "sign"://добавление пользователя
                        {
                            if (splittedAsking.Length >= 2)
                            {
                                nickname = splittedAsking[1].Trim('@');
                                if (users.Find(user => user.nickname == nickname) == null)
                                {
                                    users.Add(new User(nickname));
                                    Console.WriteLine("User " + nickname + " signed");
                                    refillUsersBase();
                                }
                                else
                                {
                                    Console.WriteLine("User " + nickname + " already signed");
                                }
                            }
                        }
                        break;

                    case "logout"://выход пользователя и удаление его из базы залогинившихся
                        {
                            if (splittedAsking.Length >= 2)
                            {
                                nickname = splittedAsking[1].Trim('@');
                                if (users.Find(user => user.nickname == nickname) != null)
                                {
                                    users.Remove(new User(nickname));
                                    refillUsersBase();
                                    break;
                                }
                            }
                        } 
                        Console.WriteLine("User "+ nickname + " not found");
                        break;

                    case "add-mes"://добавление сообщения 
                        if(splittedAsking.Length>=3)
                            foreach (User user in users)
                            {
                                nickname = splittedAsking[1].Trim('@');
                                if (nickname == user.nickname)
                                {
                                    for (int i = 2; i < splittedAsking.Length; i++)
                                    {
                                        message += " " + splittedAsking[i];
                                    }
                                    messagesTurples.Add((++messageID, DateTime.Now.ToString(), user.nickname, asking));
                                    messagesTurples.Add((++messageID, DateTime.Now.ToString(), user.nickname, message));
                                    Console.WriteLine(messagesTurples.Last().dateTime+" | "+messagesTurples.Last().nickname +" : "+ messagesTurples.Last().message);
                                    break;
                                }
                            }
                        break;
                    case "del-mes"://удаление сообщения
                        if (splittedAsking.Length >= 2)
                            foreach (User user in users)
                            {
                                DateTime dt = new DateTime();
                                nickname = splittedAsking[1].Trim('@');
                                int id = Int32.Parse(splittedAsking[2].Trim('@'));
                                if (nickname == user.nickname)
                                {
                                    foreach (var messageTurple in messagesTurples)
                                    {
                                        Console.WriteLine(messageTurple.dateTime.Split(' ')[0]);
                                        Console.WriteLine(DateTime.Now.ToString().Split(' ')[0]);
                                        if (messageTurple.ID == id)
                                            if(messageTurple.dateTime.Split(' ')[0]==DateTime.Now.ToString().Split(' ')[0])
                                            { 
                                                messagesTurples.Remove(messageTurple);
                                                refillHistoryBase();
                                            }
                                        break;
                                    }
                                }
                            }
                        break;

                    case "bot"://обращение к боту
                        if (splittedAsking.Length >= 4)
                            foreach (Bot onebot in bots)
                            {
                                botName = splittedAsking[1].Trim('@');
                                nickname = splittedAsking[2].Trim('@');
                            
                                if (onebot.botName == botName)
                                {
                                    foreach (User user in users)
                                    {
                                        if (user.nickname == nickname)
                                        {
                                            string askToBot = String.Empty;
                                            for (int i = 3; i < splittedAsking.Length; i++)
                                                askToBot += splittedAsking[i];//склеиваем комманду без адреса
                                            message = onebot.TakeAnswer(askToBot);

                                            if (message != null)
                                            {
                                                messagesTurples.Add((++messageID, DateTime.Now.ToString(), user.nickname,  asking));
                                                messagesTurples.Add((++messageID, DateTime.Now.ToString(), onebot.botName,  message));
                                                Console.WriteLine(messagesTurples.Last().dateTime + " | " + messagesTurples.Last().nickname + " : " + messagesTurples.Last().message);
                                            }
                                            break;
                                        }
                                    }
                                } 
                            }
                        break;

                    case "stop-chat":
                        {
                            users.Clear();
                            refillHistoryBase();
                            refillUsersBase();
                            isFinal = true;
                        }
                        break;
                }
                message = String.Empty;
                refillHistoryBase();
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
        /// Получаем историю сообщений из бинарного файла
        /// </summary>
        static void getHistoryBase()
        {
            using (BinaryReader reader = new BinaryReader(File.Open("../../../Binary/history", FileMode.OpenOrCreate)))
            {
                while (reader.PeekChar() > -1)
                {
                    int ID = reader.ReadInt32();
                    string dateTime = reader.ReadString();
                    string nickname = reader.ReadString();
                    string message = reader.ReadString();
                    messagesTurples.Add((ID,dateTime,nickname,message));
                }

                
            }
            if(messagesTurples.Count!=0) 
                messageID = messagesTurples.Last().ID;
        }

        /// <summary>
        /// Перезаписать файл истории
        /// </summary>
        static void refillHistoryBase()
        {
            using (BinaryWriter writer = new BinaryWriter(File.Open("../../../Binary/history", FileMode.Create)))
            {
                foreach(var messageTurple in messagesTurples)
                if (messagesTurples.Count != 0)
                {
                    writer.Write(messageTurple.ID);
                    writer.Write(messageTurple.dateTime);
                    writer.Write(messageTurple.nickname);
                    writer.Write(messageTurple.message);
                }
            }
        }

        /// <summary>
        /// Получаем базу неразлогинившихся пользователей
        /// </summary>
        static void getUsersBase()
        {
            using (BinaryReader reader = new BinaryReader(File.Open("../../../Binary/users", FileMode.OpenOrCreate)))
            {
                while (reader.PeekChar() > -1)
                {
                    User user = new User(reader.ReadString());
                    users.Add(user);
                }
            }
        }

        /// <summary>
        /// Перезаписываем базу пользователей
        /// </summary>
        static void refillUsersBase()
        {
            using (BinaryWriter writer = new BinaryWriter(File.Open("../../../Binary/users", FileMode.Create)))
            {
                foreach(var user in users)
                    writer.Write(user.nickname);
            }
        }

        /// <summary>
        /// сохранение фразы любого типа в файл
        /// </summary>
        /// <param name="fileName">Имя-тип XML файла</param>
        /// <param name="text">Добавляемый</param>
        static void putXML(string fileName, string text)
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
        static void putXML(string fileName, string text, string IDs)
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

        /// <summary>
        /// получение всех ответов из базы
        /// </summary>
        /// <param name="questions">Лист объектов-вопросов</param>
        /// <param name="fileName">Имя файла</param>
        static void GetXML(List<Question> questions, String fileName)
        {
            Question qBuff;
            var xDoc = new XmlDocument();
            string pathXMLFolder = "../../../XML/";
            xDoc.Load(pathXMLFolder + fileName + ".xml");
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
        static void GetXML(List<Answer> answers, String fileName)
        {
            Answer aBuff;
            var xDoc = new XmlDocument();
            string pathXMLFolder = "../../../XML/";
            xDoc.Load(pathXMLFolder + fileName + ".xml");
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
