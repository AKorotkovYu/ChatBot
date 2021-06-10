
namespace ClassLibraryChat
{
    public class Message
    {
        private static int newMessageID;
        public int MessageID { get; set; }
        public string DateTime { get; set; }
        public string Nickname { get; set; }
        public string Text { get; set; }
        
        static Message()
        {
            newMessageID = 0;
        }

        public Message(string dateTime, string nickname, string text)
        {
            MessageID = newMessageID++;
            DateTime = dateTime;
            Nickname = nickname;
            Text = text;
        }

    }
}
