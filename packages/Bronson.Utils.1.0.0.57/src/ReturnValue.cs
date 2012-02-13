using System;
using System.Collections.Generic;

namespace Bronson.Utils
{
    public interface IReturnValue
    {
        string Message { get; set; }
        bool Succeeded { get; set; }
        bool HasWarning { get; set; }
        List<UIMessage> AdditionalMessages { get; set; }
        void SetFailMessage(string msg);
        void SetFailMessage(string msg, params object[] args);
        void MergeMessages(IReturnValue retVal);
    }

    [Serializable]
    public class ReturnValue : IReturnValue
    {
        public ReturnValue() { }
        public ReturnValue(bool bSucceeded, string sMessage)
        {
            this.Succeeded = bSucceeded;
            this.Message = sMessage;
        }

        public void SetFailMessage(string msg)
        {
            this.Succeeded = false;
            this.Message = msg;
        }
        public void SetFailMessage(string msg, params object[] args)
        {
            this.Succeeded = false;
            this.Message = string.Format(msg, args);
        }

        private bool succeeded = true;
        public bool Succeeded
        {
            get { return this.succeeded; }
            set { succeeded = value; }
        }

        private bool hasWarning = false;
        public bool HasWarning
        {
            get { return this.hasWarning; }
            set { hasWarning = value; }
        }

        private string message = "Operation Completed Successfully";
        public string Message
        {
            get { return this.message; }
            set { message = value; }
        }

        private List<UIMessage> additionalMessages = new List<UIMessage>();
        public List<UIMessage> AdditionalMessages
        {
            get { return this.additionalMessages; }
            set { additionalMessages = value; }
        }

        public void MergeMessages(IReturnValue retVal)
        {
            if (retVal != null)
            {
                if ((retVal.Succeeded) && (retVal.HasWarning))
                    AdditionalMessages.Add(new UIMessage(retVal.Message, UIMessage.MessageTypes.Warning));
                else if (retVal.Succeeded)
                    AdditionalMessages.Add(new UIMessage(retVal.Message, UIMessage.MessageTypes.General));
                else
                    AdditionalMessages.Add(new UIMessage(retVal.Message, UIMessage.MessageTypes.Error));

                foreach (UIMessage msg in retVal.AdditionalMessages)
                    AdditionalMessages.Add(msg);
            }
        }

        public static bool operator true(ReturnValue operand)
        {
            return operand.Succeeded;
        }
        public static bool operator false(ReturnValue operand)
        {
            return operand.Succeeded == false;
        }

    }

    [Serializable]
    public class ReturnValue<T> : IReturnValue
    {

        public ReturnValue() { }
        public ReturnValue(bool bSucceeded, string sMessage, T objValue)
        {
            this.Succeeded = bSucceeded;
            this.Message = sMessage;
            this.Value = objValue;
        }
        public ReturnValue(ReturnValue simpleRetVal, T objValue)
        {
            this.Succeeded = simpleRetVal.Succeeded;
            this.Message = simpleRetVal.Message;
            this.AdditionalMessages = simpleRetVal.AdditionalMessages;
            this.HasWarning = simpleRetVal.HasWarning;
            this.Value = objValue;
        }

        public void SetFailMessage(string msg)
        {
            this.Succeeded = false;
            this.Message = msg;
        }
        public void SetFailMessage(string msg, params object[] args)
        {
            this.Succeeded = false;
            this.Message = string.Format(msg, args);
        }

        private bool succeeded = true;
        public bool Succeeded
        {
            get { return this.succeeded; }
            set { succeeded = value; }
        }

        private bool hasWarning = false;
        public bool HasWarning
        {
            get { return this.hasWarning; }
            set { hasWarning = value; }
        }

        private string message = "Operation Completed Successfully";
        public string Message
        {
            get { return this.message; }
            set { message = value; }
        }

        private T _value;
        public T Value
        {
            get { return this._value; }
            set { _value = value; }
        }

        private List<UIMessage> additionalMessages = new List<UIMessage>();
        public List<UIMessage> AdditionalMessages
        {
            get { return this.additionalMessages; }
            set { additionalMessages = value; }
        }

        public void MergeMessages(IReturnValue retVal)
        {
            if (retVal != null)
            {
                if ((retVal.Succeeded) && (retVal.HasWarning))
                    AdditionalMessages.Add(new UIMessage(retVal.Message, UIMessage.MessageTypes.Warning));
                else if (retVal.Succeeded)
                    AdditionalMessages.Add(new UIMessage(retVal.Message, UIMessage.MessageTypes.General));
                else
                    AdditionalMessages.Add(new UIMessage(retVal.Message, UIMessage.MessageTypes.Error));

                foreach (UIMessage msg in retVal.AdditionalMessages)
                    AdditionalMessages.Add(msg);
            }

        }

        public static bool operator true(ReturnValue<T> operand)
        {
            return operand.Succeeded;
        }
        public static bool operator false(ReturnValue<T> operand)
        {
            return operand.Succeeded == false;
        }

    }
}
