using System;
using ClassLibraryChatBot;

namespace ConsoleChatBot
{
    class Program
    {
        static void Main(string[] args)
        {
            Bot bot = new Bot();
            System.Console.WriteLine(bot.TakeAnswer("Привет"));

            string s = new String("");
            while (bot.isFinal == false)
            {
                System.Console.WriteLine("");
                s = Console.ReadLine();
                System.Console.WriteLine("");
                System.Console.WriteLine(bot.TakeAnswer(s));
            }

        }
    }
}
