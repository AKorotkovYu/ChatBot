using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using ClassLibraryChatBot;

namespace ConsoleChatBot
{
    public struct MessageInfo
    {
        public int ID { get; set; }
        public string DateTime { get; set; }
        public string Nickname { get; set; }
        public string Message { get; set; }

        public MessageInfo(int ID, string dateTime, string nickname, string message)
        {
            this.ID = ID;
            DateTime = dateTime;
            Nickname = nickname;
            Message = message;
        }
    }

    class Chat
    {
        string chatName;
        public string ChatName { get { return chatName; } }

        List<MessageInfo> messages = new List<MessageInfo>();
        readonly List<User> users = new List<User>();
        readonly List<Bot> bots = new List<Bot>();

        static string message = String.Empty;
        static int messageID = 0;

        string usersPath;
        string historyPath;

        public Chat(string chatName, List<Bot> bots, string historyPath, string usersPath)
        {
            this.chatName = chatName;
            this.bots = bots;
            this.historyPath = historyPath;
            this.usersPath = usersPath;
        }

        Chat()
        {

        }

        bool changeChatName(User user, string chatName)
        {
            foreach(User oneUser in users)
            {
                if(oneUser.nickname==user.nickname)
                {
                    this.chatName = chatName;
                    return true;
                }
            }
            return false;
        }

        public bool SignUser(string ask)
        {
            var splittedAsking = ask.Split().ToArray();
            string nickname;

            if (splittedAsking.Length >= 2)
            {
                nickname = splittedAsking[1].Trim('@');
                if (users.Find(user => user.nickname == nickname) == null)
                {
                    users.Add(new User(nickname));
                    this.RefillUsersBase(usersPath);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
                return false;
        }

        public List<string> GetAllUsers()
        {
            List<string> allUsers = new List<string>();
            foreach (var user in users)
                allUsers.Add(user.nickname);
            return allUsers;
        }

        public List<MessageInfo> GetHistory()
        {
            List<MessageInfo> history = new List<MessageInfo>();
            foreach (var message in messages)
                history.Add(message);
            return history;
        }

        public bool LogOut(string ask)
        {
            var splittedAsking = ask.Split().ToArray();
            string nickname;

            if (splittedAsking.Length >= 2)
            {
                nickname = splittedAsking[1].Trim('@');
                if (users.Find(user => user.nickname == nickname) != null)
                {
                    RefillUsersBase(usersPath);
                    users.Remove(new User(nickname));
                    return true;
                }
            }
            return false;
        }

        public bool AddMes(string ask)
        {
            var splittedAsking = ask.Split().ToArray();
            string nickname;

            if (splittedAsking.Length >= 3)
                foreach (User user in users)
                {
                    nickname = splittedAsking[1].Trim('@');
                    if (nickname == user.nickname)
                    {
                        for (int i = 2; i < splittedAsking.Length; i++)
                        {
                            message += " " + splittedAsking[i];
                        }
                        messages.Add(new MessageInfo(++messageID, DateTime.Now.ToString(), user.nickname, ask));
                        messages.Add(new MessageInfo(++messageID, DateTime.Now.ToString(), user.nickname, message));
                        message = string.Empty;
                        return true;
                        
                    }
                }
            return false;
        }

        public bool DelMes(string ask)
        {
            var splittedAsking = ask.Split().ToArray();
            string nickname;

            if (splittedAsking.Length >= 2)
                foreach (User user in users)
                {
                    nickname = splittedAsking[1].Trim('@');
                    int id = Int32.Parse(splittedAsking[2].Trim('@'));
                    if (nickname == user.nickname)
                    {
                        foreach (var message in messages)
                        {
                            Console.WriteLine(message.DateTime.Split(' ')[0]);
                            Console.WriteLine(DateTime.Now.ToString().Split(' ')[0]);
                            if (message.ID == id)
                                if (message.DateTime.Split(' ')[0] == DateTime.Now.ToString().Split(' ')[0])
                                {
                                    messages.Remove(message);
                                    RefillHistoryBase(historyPath);
                                    return true;
                                }
                        }
                        return false;
                    }
                }
            return false;
        }

        public string BotCommand(string ask)
        {
            var splittedAsking = ask.Split().ToArray();
            string nickname;
            string botName;

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
                                    messages.Add(new MessageInfo(++messageID, DateTime.Now.ToString(), user.nickname, ask));
                                    messages.Add(new MessageInfo(++messageID, DateTime.Now.ToString(), onebot.botName, message));
                                    return messages.Last().Message;
                                }
                                break;
                            }
                        }
                    }
                }
            return "ERROR";
        }

        public bool StopChat()
        {
            users.Clear();
            if((RefillHistoryBase("../../../Binary"))&
                (RefillUsersBase("../../../Binary")))
                    return true;
            return false;
        }

        /// <summary>
        /// Получаем историю сообщений из бинарного файла
        /// </summary>
        bool GetHistoryBase(string path)
        {
            try
            {
                using BinaryReader reader = new BinaryReader(File.Open(path + @"/" + chatName + "history", FileMode.OpenOrCreate));
                while (reader.PeekChar() > -1)
                {
                    int ID = reader.ReadInt32();
                    string dateTime = reader.ReadString();
                    string nickname = reader.ReadString();
                    string message = reader.ReadString();
                    messages.Add(new MessageInfo(ID, dateTime, nickname, message));
                }
                if (messages.Count != 0)
                {
                    messageID = messages.Last().ID;
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Перезаписать файл истории
        /// </summary>
        public bool RefillHistoryBase(string path)
        {
            try
            {
                using BinaryWriter writer = new BinaryWriter(File.Open(path + @"/" + chatName + "history", FileMode.Create));
                foreach (var messageTurple in messages)
                    if (messages.Count != 0)
                    {
                        writer.Write(messageTurple.ID);
                        writer.Write(messageTurple.DateTime);
                        writer.Write(messageTurple.Nickname);
                        writer.Write(messageTurple.Message);
                    }
            }
            catch(Exception)
            {
                return false;
            }
            return true;
         }

        /// <summary>
        /// Получаем базу неразлогинившихся пользователей
        /// </summary>
        public bool GetUsersBase(string path)
        {
            try
            {
                using BinaryReader reader = new BinaryReader(File.Open(path + @"/" + chatName + "users", FileMode.OpenOrCreate));
                while (reader.PeekChar() > -1)
                {
                    User user = new User(reader.ReadString());
                    users.Add(user);
                }
                
            }
            catch(Exception)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Перезаписываем базу пользователей
        /// </summary>
        public bool RefillUsersBase(string path)
        {
            try
            {
                using BinaryWriter writer = new BinaryWriter(File.Open(path + @"/" + chatName + "users", FileMode.Create));
                {
                    foreach (var user in users)
                        writer.Write(user.nickname);
                }
            }
            catch(Exception)
            {
                return false;
            }
            return true;
        }
    }
}
