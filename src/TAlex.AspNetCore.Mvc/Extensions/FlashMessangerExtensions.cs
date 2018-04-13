using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
            var messages = controller.TempData.Peek(FlashMessangerSessionKey) as IDictionary<FlashMessageType, Queue<string>>;

            if (messages == null)
            {
                messages = InitializeMessages();
                controller.TempData[FlashMessangerSessionKey] = messages;
            }

            messages[type].Enqueue(text);
        }

        public static IHtmlContent FlashMessages(this IHtmlHelper source, FlashMessageType? messageType = null)
        {
            var messages = source.TempData[FlashMessangerSessionKey] as IDictionary<FlashMessageType, Queue<string>>;

            var output = new StringBuilder();

            if (messages != null)
            {
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

        #endregion
    }
}
