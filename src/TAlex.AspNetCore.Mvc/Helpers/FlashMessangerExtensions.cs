using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ViewFeatures;


namespace TAlex.Web.Mvc.Helpers
{
    public static class FlashMessangerExtensions
    {
        public static HtmlString FlashMessages(this HtmlHelper source)
        {
            return FlashMessanger.GetCurrent(source.ViewContext).RetrieveMessages();
        }

        public static HtmlString FlashMessages(this HtmlHelper source, FlashMessanger.MessageType messageType)
        {
            return FlashMessanger.GetCurrent(source.ViewContext).RetrieveMessages(messageType);
        }
    }
}
