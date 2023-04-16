using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace ClassLibraryChat
{
    public class User
    {
        private static int newID;
        int ID;
        public bool IsOnline { get; set; }
        static User()
        {
            newID=0;
        }

        public string Nickname { get; set; }

        private protected List<Message> messages = new List<Message>();
        private protected int messageID = 0;
        readonly private protected string historyPath;



        public User(string nickname,string historyPath)
        {
            IsOnline = true;
            ID = newID++;
            Nickname = nickname;
            this.historyPath = historyPath + @"/UsersHistory/" + nickname;
            GetHistoryBase(this.historyPath);
        }

        public Message SendMessage(string text)
        {
            if(IsOnline==true)
            { 
               messages.Add(new Message(DateTime.Now.ToString(), this.Nickname, text));
               RefillHistoryBase(historyPath);
               return messages.Last();
            }
            return null;
        }

        public Message FindMessage(int id)
        {
            var message = messages.Find(mes => mes.MessageID == id);
            return message;
        }

        public bool DelMes(Message message)
        {
            if (message.DateTime.Split(' ')[0] == DateTime.Now.ToString().Split(' ')[0])
            {
                messages.Remove(message);
                RefillHistoryBase(historyPath);
                return true;
            }
            return false;
        }

        public List<Message> GetHistory()
        {
            List<Message> history  = new List<Message>();
            foreach (var message in messages)
                history.Add(message);
            return history;
        }

        /// <summary>
        /// Получаем историю сообщений из бинарного файла
        /// </summary>
        protected bool GetHistoryBase(string path)
        {
            try
            {
                DirectoryInfo dirInfo = new DirectoryInfo(path);
                if (!dirInfo.Exists)
                {
                    dirInfo.Create();
                }

                using BinaryReader reader = new BinaryReader(File.Open(path + $"/{Nickname}History", FileMode.OpenOrCreate));
                while (reader.PeekChar() > -1)
                {
                    string dateTime = reader.ReadString();
                    string nickname = reader.ReadString();
                    string message = reader.ReadString();
                    messages.Add(new Message(dateTime, nickname, message));
                }
                if (messages.Count != 0)
                {
                    messageID = messages.Last().MessageID;
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        /// <summary>
        /// Перезаполняем историю сообщений в бинарном файле
        /// </summary>
        /// <param name="path">путь до папки с бинарным файлом</param>
        /// <returns></returns>
        protected bool RefillHistoryBase(string path)
        {
            try
            {
                using BinaryWriter writer = new BinaryWriter(File.Open(path + @"/" + Nickname + "history", FileMode.Create));
                foreach (var messageTurple in messages)
                    if (messages.Count != 0)
                    {
                        writer.Write(messageTurple.DateTime);
                        writer.Write(messageTurple.Nickname);
                        writer.Write(messageTurple.Text);
                    }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
    }
}
