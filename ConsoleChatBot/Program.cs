using System;
using System.Xml;
using System.Linq;
using System.Collections;
using ClassLibraryChatBot;
using System.Collections.Generic;

namespace ConsoleChatBot
{
    class Program
    {
        static Dictionary<Question, Bot.CommandMessage> commands = new Dictionary<Question, Bot.CommandMessage>();
        static Random randomizer = new Random();

        static void Main()
        { 
            List<Answer> answersList = new List<Answer>();
            List<Question> questionsList = new List<Question>();

            GetXML(answersList, "jok");
            GetXML(answersList, "aph");
            GetXML(answersList, "met");
            GetXML(questionsList, "que");
            GetXML(answersList, "ini");
            GetCommands(commands);

            Bot bot = new Bot(questionsList,answersList,commands);

            string asking = string.Empty;
            string ans = string.Empty;

            while (ans!="до свидания")
            {
                asking = Console.ReadLine();
                ans = bot.TakeAnswer(asking);
                System.Console.WriteLine(ans);
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
        /// <param name="fileName"></param>
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


        /// <summary>
        /// Создание списка возможных комманд
        /// </summary>
        /// <param name="Commands">Словарь, состоящий из команды и делегата-действия</param>
        static void GetCommands(Dictionary<Question, Bot.CommandMessage> Commands)
        {
            Question qBuff;

            Commands.Add(qBuff = new Question("как тебя зовут"), () => { return "Меня зовут " + Bot.botName; });//9
            Commands.Add(qBuff = new Question("какая сейчас дата"), () => { return DateTime.Now.ToString().Split(" ").ToArray()[1]; });//11
            Commands.Add(qBuff = new Question("который час"), () => { return DateTime.Now.ToString().Split(" ").ToArray()[1]; });//12
            Commands.Add(qBuff = new Question("сколько времени"), () => { return DateTime.Now.ToString().Split(" ").ToArray()[0]; });//15
            Commands.Add(qBuff = new Question("брось кубик"), () => { return randomizer.Next(0, 6).ToString(); });//16
            Commands.Add(qBuff = new Question("подбрось кубик"), () => { return randomizer.Next(0, 6).ToString(); });//17
            Commands.Add(qBuff = new Question("подбрось монетку"), () => { return (randomizer.Next(0, 6) == 1) ? "Орёл" : "Режка"; });//18
            Commands.Add(qBuff = new Question("брось монетку"), () => { return (randomizer.Next(0, 6) == 1) ? "Орёл" : "Режка"; });//19
            Commands.Add(qBuff = new Question("до свидания"), () =>
            {
                return "до cвидания";
            }
                );
            Commands.Add(qBuff = new Question("пока"), () =>
            {
                return "до cвидания";
            }
                );
        }

    }
}
