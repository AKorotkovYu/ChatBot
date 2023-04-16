using System;
using System.Linq;
using System.Xml;
using ClassLibraryChat;
using System.Collections.Generic;

namespace ConsoleChatBot
{
    class Program
    {
        static Random randomizer = new Random();
        static List<Bot> bots = new List<Bot>();

        static void Main()
        {

            string ask = String.Empty;
            bool isFinal = true;

            string XMLFolder = "../../../XML";
            string usersFolder = "../../../Binary";
            string historyFolder = "../../../Binary";

            //Комманды на вход боту


            do
            {
                System.Console.WriteLine("enter 'start-chat'");
                ask = Console.ReadLine();
                isFinal = false;
            }
            while (ask != "start-chat");

            Chat onechat = new Chat("chat", usersFolder);

            onechat.bots.Add(new Bot("firstBot", $"{historyFolder}/{onechat.ChatName}", XMLFolder));
            onechat.bots.Add(new Bot("secondBot", $"{historyFolder}/{onechat.ChatName}", XMLFolder));
            do
            {
                ask = Console.ReadLine();
                var splittedAsking = ask.Split().ToArray();

                switch (ask.Split(" ")[0])
                {
                    case "sign"://добавление пользователя
                        {
                            if (splittedAsking.Length >= 2)
                            {
                                if (splittedAsking[1][0] == '@')
                                {
                                    var nickname = splittedAsking[1].Trim('@');
                                    User newUser = new User(nickname, $"{usersFolder}/{onechat.ChatName}");
                                    if (onechat.SignUser(newUser))
                                    {
                                        Console.WriteLine($"Ноавый пользователь {newUser.Nickname} вошёл в чат");
                                    }
                                    else
                                    {
                                        Console.WriteLine($"Пользователь {newUser.Nickname} уже добавлен в чат");
                                    }
                                }
                            }
                        }
                        break;

                    case "logout"://выход пользователя и удаление его из базы залогинившихся
                        {
                            if (splittedAsking.Length >= 2)
                            {
                                if (splittedAsking[1][0] == '@')
                                {
                                    var nickname = splittedAsking[1].Trim('@');
                                    if (onechat.LogOut(nickname))
                                    {
                                        Console.WriteLine($"Пользователь {nickname} покинул чат");
                                    }
                                    else
                                    {
                                        Console.WriteLine($"Пользователь {nickname} покинул чат");
                                    }
                                }
                            }
                        }
                        break;

                    case "add-mes"://добавление сообщения 
                        {
                            
                            if (splittedAsking[1][0] == '@')
                            {
                                var nickname = splittedAsking[1].Trim('@');
                                string text = string.Empty;

                                for (int i = 2; i < splittedAsking.Length; i++)
                                    text += splittedAsking[i];
                                Message message = onechat.FindUser(nickname).SendMessage(text);
                                if (message != null)
                                {
                                    Console.WriteLine($"{message.DateTime}: {message.Nickname} - {message.Text} ");
                                }
                                else
                                {
                                    Console.WriteLine("Сообщение не отправлено");
                                }
                            }
                        }
                        break;

                    case "view-his"://просмотреть все сообщения в истории
                        {
                            if (splittedAsking.Length >= 2)
                            {
                                if (splittedAsking[1][0] == '@')
                                {
                                    var nickname = splittedAsking[1].Trim('@');
                                    var massageHistory = onechat.FindUser(nickname).GetHistory();

                                    foreach (var message in massageHistory)
                                    {
                                        Console.WriteLine($"Message ID: {message.MessageID}, {message.DateTime}: {message.Nickname} - {message.Text} ");
                                    }
                                }
                            }
                        }
                        break;

                    case "del-mes"://удаление сообщения
                        {
                            if (splittedAsking.Length >= 3)
                            {
                                if (splittedAsking[1][0] == '@')
                                {
                                    var nickname = splittedAsking[1].Trim('@');

                                    if (Int32.TryParse(splittedAsking[2], out int id))
                                    {
                                        var user = onechat.FindUser(nickname);

                                        if (user.DelMes(user.FindMessage(id)))
                                        {
                                            Console.WriteLine("Сообщение удалено");
                                        }
                                        else
                                        {
                                            Console.WriteLine("Сообщение не найдено или написано не сегодня");
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("Неправильный ID");
                                    }
                                }
                            }
                        }
                        break;
                    case "all-users":
                        {
                            List<User> users = onechat.GetAllUsers();
                            foreach(User user in users)
                            {
                                string isOnline = String.Empty;
                                if (user.IsOnline == true)
                                    isOnline = "Online";
                                else
                                    isOnline = "Offline";
                                Console.WriteLine($"{user.Nickname}:  {isOnline}");
                            }
                        }
                        break;

                    case "bot"://обращение к боту
                        {
                            if (splittedAsking.Length >= 2)
                            {
                                var botname = splittedAsking[1].Trim('@');
                                var nickname = splittedAsking[2].Trim('@');
                                var command = ask.Replace($"bot {botname} {nickname} ", String.Empty);

                                User user = onechat.FindUser(nickname);
                                if (user != null)
                                    foreach (Bot bot in onechat.bots)
                                        if (bot.Nickname.ToLower() == botname.ToLower())
                                        {
                                            Message message = bot.SendMessage(user, command);
                                            Console.WriteLine($"{message.DateTime}: {message.Nickname} - {message.Text} ");
                                        }
                            }
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
            }
            while (!isFinal);
        }
    }
}
