using System;
using System.Collections;
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
            while (bot.IsFinal == false)
            {
                int[] i = new int[100];
                string s1 = new String("");
                dynamic d = i;
                object o = i;
                d = s;
                o = s;
                System.Console.WriteLine("");
                s = Console.ReadLine();
                System.Console.WriteLine("");
                System.Console.WriteLine(bot.TakeAnswer(s));
            }
        }
    }
}
