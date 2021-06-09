using System;
using System.Collections.Generic;
using System.IO;

namespace ClassLibraryChatBot
{
    class User
    {
        public string nickname { get; set; }

        public User(string nickname)
        {
            this.nickname = nickname;
        }
    }
}
