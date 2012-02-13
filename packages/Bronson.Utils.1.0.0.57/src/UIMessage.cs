using System;

namespace Bronson.Utils
{
    [Serializable]
    public class UIMessage
    {
        public enum MessageTypes : int
        {
            General = 101,
            Warning = 102,
            Error = 103
        }

        private string message = string.Empty;
        public string Message
        {
            get { return this.message; }
            set { message = value; }
        }

        private MessageTypes type = MessageTypes.General;
        public MessageTypes Type
        {
            get { return this.type; }
            set
            {
                type = value;
                TypeAsString = value.ToString();
            }
        }

        private string typeAsString = "General";
        public string TypeAsString
        {
            get { return this.typeAsString; }
            set { typeAsString = value; }
        }

        public UIMessage()
        {

        }

        public UIMessage(string msg)
        {
            this.Message = msg;
        }
        public UIMessage(string msg, params object[] args)
        {
            this.Message = string.Format(msg, args);

        }
        public UIMessage(string msg, MessageTypes type, params object[] args)
        {
            this.Message = string.Format(msg, args);
            this.Type = type;
        }
        public UIMessage(string msg, MessageTypes type)
        {
            this.Message = msg;
            this.Type = type;
        }


    }
}
