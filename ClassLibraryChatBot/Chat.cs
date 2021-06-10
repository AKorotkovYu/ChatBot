using System;
using System.Collections.Generic;
using System.IO;

namespace ClassLibraryChat
{

    public class Chat
    {
        private string chatName;
        public string ChatName { get { return chatName; } }

        List<User> users = new List<User>();
        public List<Bot> bots = new List<Bot>();

        private string basePath;

        public Chat(string chatName, string usersPath)
        {
            this.chatName = chatName;
            this.basePath = usersPath;
            GetUsersBase(usersPath);
        }

        Chat()
        {

        }


        /// <summary>
        /// Смена имени Чата
        /// </summary>
        /// <param name="user">Пользователь из чата</param>
        /// <param name="chatName">Новое имя чата</param>
        /// <returns>Выоплнено ли</returns>
        bool ChangeChatName(User user, string chatName)
        {
            foreach (User oneUser in users)
            {
                if (oneUser.Nickname == user.Nickname)
                {
                    this.chatName = chatName;
                    return true;
                }
            }
            return false;
        }


        /// <summary>
        /// Найти пользователя по нику
        /// </summary>
        /// <param name="nickname">Никнейм искомого пользователя</param>
        /// <returns></returns>
        public User FindUser(string nickname)
        {
           return users.Find(user => user.Nickname == nickname);
        }

        public bool SignUser(User newUser)
        {
            if (users.Find(user => user.Nickname == newUser.Nickname) == null)
            {
                users.Add(newUser);
                this.RefillUsersBase(basePath);
                return true;
            }
            else
                return false;
        }


        /// <summary>
        /// Получение всех пользователей чата в формате листа
        /// </summary>
        /// <returns>Лист с пользователями</returns>
        public List<User> GetAllUsers()
        {
            List<User> allUsers = new List<User>();
            foreach (var user in users)
                allUsers.Add(user);
            return allUsers;
        }


        /// <summary>
        /// выйти из чата-оффлайн
        /// </summary>
        /// <param name="nickname"></param>
        /// <returns></returns>
        public bool LogOut(string nickname)
        {
            FindUser(nickname).IsOnline=false;
            RefillUsersBase(basePath);
            return true;
        }

        /// <summary>
        /// Остановить чат
        /// </summary>
        /// <returns></returns>
        public bool StopChat()
        {
            if(this.RefillUsersBase(basePath))
                return true;
            return false;
        }


        /// <summary>
        /// Получаем базу неразлогинившихся пользователей
        /// </summary>
        private bool GetUsersBase(string path)
        {
            try
            {
                using BinaryReader reader = new BinaryReader(File.Open(path + @"/" + chatName + "users", FileMode.OpenOrCreate));
                while (reader.PeekChar() > -1)
                {
                    User user = new User(reader.ReadString(),path);
                    users.Add(user);
                }

            }
            catch (Exception)
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
                        writer.Write(user.Nickname);
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
