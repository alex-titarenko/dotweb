using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;


namespace TAlex.Web.Mvc.Helpers
{
    public static class FlashMessangerExtensions
    {
        public static IHtmlString FlashMessages(this HtmlHelper source)
        {
            return FlashMessanger.Current.RetrieveMessages();
        }

        public static IHtmlString FlashMessages(this HtmlHelper source, FlashMessanger.MessageType messageType)
        {
            return FlashMessanger.Current.RetrieveMessages(messageType);
        }
    }
}
