using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;


namespace TAlex.Web.Mvc.Helpers
{
    public class FlashMessanger
    {
        #region Fields

        private const string FlashMessangerSessionKey = "FlashMessanger";


        protected IDictionary<MessageType, Queue<string>> _messages;

        #endregion

        #region Properties

        public static FlashMessanger Current
        {
            get
            {
                FlashMessanger messanger = null;

                if (HttpContext.Current != null)
                {
                    messanger =
                        HttpContext.Current.Session[FlashMessangerSessionKey] as FlashMessanger;
                }

                if (messanger == null)
                {
                    messanger = new FlashMessanger();
                    if (HttpContext.Current != null)
                    {
                        HttpContext.Current.Session[FlashMessangerSessionKey] = messanger;
                    }
                }

                return messanger;
            }
        }

        #endregion

        #region Constructors

        protected FlashMessanger()
        {
            Initialization();
        }

        #endregion

        #region Methods

        private void Initialization()
        {
            _messages = new Dictionary<MessageType, Queue<string>>();

            foreach (MessageType type in Enum.GetValues(typeof(MessageType)))
            {
                _messages.Add(type, new Queue<string>());
            }
        }

        public void AddMessage(string text)
        {
            AddMessage(text, MessageType.Message);
        }

        public void AddMessage(string text, MessageType type)
        {
            _messages[type].Enqueue(text);
        }


        public IHtmlString RetrieveMessages()
        {
            StringBuilder output = new StringBuilder();

            foreach (MessageType messageType in _messages.Keys)
            {
                output.Append(RetrieveMessages(messageType));
            }

            return MvcHtmlString.Create(output.ToString());
        }

        public IHtmlString RetrieveMessages(MessageType type)
        {
            StringBuilder result = new StringBuilder();

            if (_messages[type].Count > 0)
            {
                result.AppendFormat("<ul class=\"flash-messages {0}\">", type.ToString().ToLower());

                while (_messages[type].Count > 0)
                {
                    result.AppendFormat("<li>{0}</li>", _messages[type].Dequeue());
                }

                result.Append("</ul>");
            }

            return MvcHtmlString.Create(result.ToString());
        }

        #endregion

        #region Nested Types

        public enum MessageType
        {
            Message,
            Warning,
            Error,
        }

        #endregion
    }
}
