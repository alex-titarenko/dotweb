using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;


namespace TAlex.Web.Mvc.Helpers
{
    public class FlashMessanger
    {
        #region Fields

        private const string FlashMessangerSessionKey = "FlashMessanger";


        protected IDictionary<MessageType, Queue<string>> _messages;

        #endregion

        #region Properties

        public static FlashMessanger GetCurrent(ViewContext context)
        {
            FlashMessanger messanger = null;

            if (context != null)
            {
                messanger = context.TempData[FlashMessangerSessionKey] as FlashMessanger;
            }

            if (messanger == null)
            {
                messanger = new FlashMessanger();
                if (context != null)
                {
                    context.TempData[FlashMessangerSessionKey] = messanger;
                }
            }

            return messanger;
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

        public virtual void AddMessage(string text)
        {
            AddMessage(text, MessageType.Message);
        }

        public virtual void AddMessage(string text, MessageType type)
        {
            _messages[type].Enqueue(text);
        }


        public HtmlString RetrieveMessages()
        {
            StringBuilder output = new StringBuilder();

            foreach (MessageType messageType in _messages.Keys)
            {
                output.Append(RetrieveMessages(messageType));
            }
            
            return new HtmlString(output.ToString());
        }

        public HtmlString RetrieveMessages(MessageType type)
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

            return new HtmlString(result.ToString());
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
