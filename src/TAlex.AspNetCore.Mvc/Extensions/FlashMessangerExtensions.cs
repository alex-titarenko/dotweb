using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace TAlex.AspNetCore.Mvc.Extensions
{
    public static class FlashMessangerExtensions
    {
        private const string FlashMessangerSessionKey = "FlashMessanger";


        public static void AddFlashMessage(this Controller controller, string text, FlashMessageType type = FlashMessageType.Message)
        {
            var messagesString = (string)controller.TempData.Peek(FlashMessangerSessionKey);
            var messages = DeserializeMessages(messagesString);
            messages[type].Enqueue(text);

            controller.TempData[FlashMessangerSessionKey] = SerializeMessages(messages);            
        }

        public static IHtmlContent FlashMessages(this IHtmlHelper source, FlashMessageType? messageType = null)
        {
            var messagesString = (string)source.TempData[FlashMessangerSessionKey];

            var output = new StringBuilder();

            if (!string.IsNullOrEmpty(messagesString))
            {
                var messages = DeserializeMessages(messagesString);

                if (messageType.HasValue)
                {
                    return new HtmlString(RetrieveMessages(messageType.Value, messages[messageType.Value]));
                }

                foreach (var kv in messages)
                {
                    output.Append(RetrieveMessages(kv.Key, kv.Value));
                }
            }

            return new HtmlString(output.ToString());
        }

        #region Helpers

        private static string RetrieveMessages(FlashMessageType type, Queue<string> messages)
        {
            var result = new StringBuilder();

            if (messages.Any())
            {
                result.AppendFormat("<ul class=\"flash-messages {0}\">", type.ToString().ToLower());

                while (messages.Any())
                {
                    result.AppendFormat("<li>{0}</li>", messages.Dequeue());
                }

                result.Append("</ul>");
            }

            return result.ToString();
        }

        private static Dictionary<FlashMessageType, Queue<string>> InitializeMessages()
        {
            var messages = new Dictionary<FlashMessageType, Queue<string>>();

            foreach (FlashMessageType type in Enum.GetValues(typeof(FlashMessageType)))
            {
                messages.Add(type, new Queue<string>());
            }

            return messages;
        }

        private static string SerializeMessages(IDictionary<FlashMessageType, Queue<string>> messages)
        {
            return JsonConvert.SerializeObject(messages);
        }

        private static IDictionary<FlashMessageType, Queue<string>> DeserializeMessages(string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                return JsonConvert.DeserializeObject<Dictionary<FlashMessageType, Queue<string>>>(str);
            }
            else
            {
                return InitializeMessages();
            }
        }

        #endregion
    }
}
