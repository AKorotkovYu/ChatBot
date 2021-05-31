using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.IO;


namespace ClassLibraryChatBot
{
    public class Bot
    {
        private static string m_BotName = "Шарпик";
        private string m_UserName = new String("");
        private bool m_IsFinal = false;

        public bool IsFinal
        {
            get { return m_IsFinal; }
        }

        Random randomizer = new Random();

        internal delegate string CommandMessage();

         readonly List<Answer> Jokes = new List<Answer>();
         readonly List<Answer> Aphorisms = new List<Answer>();
         readonly List<Answer> Meetings = new List<Answer>();
         readonly List<Answer> Answers = new List<Answer>();
         readonly List<Answer> InitPhrases = new List<Answer>();
         readonly List<Answer> Errors = new List<Answer>();

         readonly Dictionary<Question, CommandMessage> Commands = new Dictionary<Question, CommandMessage>();
         readonly List<Question> Questions = new List<Question>();

        static readonly List<String> Phrases = new List<string>();

        public Bot()
        {
            GetBase();
        }

        //главный метод, принимающий вопрос пользователя и выдающиий подходящий ответ из базы
        public string TakeAnswer(string asking)
        {

            asking = asking.ToLower();
            string answer_line = String.Empty;
            Phrases.Clear();

            foreach (var command in Commands)
                if (command.Key.Phrase == asking)
                {
                    return command.Value();
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
                    va ??= "NAN";
                    return va;
                }
            }
            answer_line = TakeRandAnswer(InitPhrases);
            string secondpart = TakeRandAnswer(Aphorisms);
            answer_line += " ";
            answer_line += secondpart;
            return answer_line;
        }


        //выбрать случайный ответ из выборки ответов
        string TakeRandFromList(List<String> lPhrases)
        {
            int r = randomizer.Next(0, lPhrases.Count());
            var an = lPhrases.Skip(r).Take(1).ToArray();
            return an[0];
        }

        //выбрать случайный ответ из всей переданной базы
        string TakeRandAnswer(List<Answer> lAnswer)
        {
            int r = randomizer.Next(0, lAnswer.Count());
            var an1 = lAnswer.Skip(r).Take(1).ToArray() ?? Answers.Where(i => i.AnswerType == "err").ToArray();
            return an1[0].Phrase;
        }

        private void GetXMKBase(string questionsPath, string answersPath)
        {

        }

        //выгрузка базы со всеми вопросами-ответами
        private void GetXMLAnswers(List<Answer> answers, String fileName)
        {
            Answer aBuff;
            List<String> textBuff = new List<string>();
            XmlDocument xDoc = new XmlDocument();
            string pathXMLFolder = "../../../XML/";
            xDoc.Load(pathXMLFolder + fileName + ".xml");
            XmlElement xRoot = xDoc.DocumentElement;
            textBuff.Clear();

            foreach (XmlNode xNode in xRoot)
            {
                if (xNode.Attributes.Count > 0)
                {
                    if(fileName=="ans")
                    {
                        XmlNode attrtext = xNode.Attributes.GetNamedItem("text");
                        if (attrtext != null)
                        {
                            XmlNode attrID = xNode.Attributes.GetNamedItem("CorrectIDsAnswers");
                            if (attrID != null)
                            {
                                string[] splitedAttr=attrID.Value.ToString().Split(" ");
                                List<int> questionIDs = new List<int>();
                                foreach (var i in splitedAttr)
                                {
                                    if(i!="")
                                        questionIDs.Add(Int32.Parse(i));
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

        private void GetXMLQuestions(List<Question> questions, String fileName)
        {
            Question qBuff;
            List<String> textBuff = new List<string>();
            XmlDocument xDoc = new XmlDocument();
            string pathXMLFolder = "../../../XML/";
            xDoc.Load(pathXMLFolder + fileName + ".xml");
            XmlElement xRoot = xDoc.DocumentElement;
            textBuff.Clear();

            int i = 0;
            foreach (XmlNode xNode in xRoot)
            {
                if (xNode.Attributes.Count > 0)
                {
                    XmlNode attr = xNode.Attributes.GetNamedItem("text");
                    if (attr != null)
                    {
                        textBuff.Add(attr.Value);
                        i++;
                    }
                }
            }

            switch(fileName)
            {
                /*case "com":
                    {
                        Commands.Add(qBuff = new Question(textBuff[0]), () => { return "Меня зовут " + m_BotName; });//9
                        Commands.Add(qBuff = new Question(textBuff[1]), () => { return TakeRandAnswer(Jokes); });//10
                        Commands.Add(qBuff = new Question(textBuff[2]), () => { return DateTime.Now.ToString().Split(" ").ToArray()[1]; });//11
                        Commands.Add(qBuff = new Question(textBuff[3]), () => { return DateTime.Now.ToString().Split(" ").ToArray()[1]; });//12
                        Commands.Add(qBuff = new Question(textBuff[4]), () => { return DateTime.Now.ToString().Split(" ").ToArray()[0]; });//15
                        Commands.Add(qBuff = new Question(textBuff[5]), () => { return randomizer.Next(0, 6).ToString(); });//16
                        Commands.Add(qBuff = new Question(textBuff[6]), () => { return randomizer.Next(0, 6).ToString(); });//17
                        Commands.Add(qBuff = new Question(textBuff[7]), () => { return (randomizer.Next(0, 6) == 1) ? "Орёл" : "Режка"; });//18
                        Commands.Add(qBuff = new Question(textBuff[8]), () => { return (randomizer.Next(0, 6) == 1) ? "Орёл" : "Режка"; });//19
                        Commands.Add(qBuff = new Question(textBuff[9]), () =>
                        {
                            m_IsFinal = true;
                            if (m_UserName != "")
                                return "до cвидания, " + m_UserName;
                            else
                                return "до cвидания";
                        }
                           );
                        Commands.Add(qBuff = new Question(textBuff[10]), () =>
                        {
                            m_IsFinal = true;
                            if (m_UserName != "")
                                return "до cвидания, " + m_UserName;
                            else
                                return "до cвидания";
                        }
                           );
                    }
                    break;
                */
                default:
                    {
                        foreach(var text in textBuff)
                            questions.Add(qBuff = new Question(text));
                    }
                    break;
            }
        }

        private void GetBase()
        {
            string fileName = "que";
            GetXMLQuestions(Questions, fileName);

            //fileName = "com";
            //GetXMLQuestions(Commands, fileName);

            fileName = "ans";
            GetXMLAnswers(Meetings, fileName);

            fileName = "jok";
            GetXMLAnswers(Jokes,fileName);

            fileName = "aph";
            GetXMLAnswers(Aphorisms,fileName);

            fileName = "ini";
            GetXMLAnswers(InitPhrases, fileName);

            fileName = "err";
            GetXMLAnswers(Errors, fileName);
        }
    }
}
