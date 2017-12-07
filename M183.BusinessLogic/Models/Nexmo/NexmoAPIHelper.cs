using System.Net;
using System.Web;
using System.Web.Script.Serialization;

namespace M183.BusinessLogic.Models.Nexmo
{
    class NexmoAPIHelper
    {
        public string Sender { get; set; }

        public NexmoAPIHelper()
        {
        }


        public NexmoResponse SendSMS(string to, string text)
        {
            var wc = new WebClient() { BaseAddress = "https://rest.nexmo.com/sms/json" };
            wc.QueryString.Add("api_key", "5e4521b6");
            wc.QueryString.Add("api_secret", "d6b2d10b06f2a697");
            wc.QueryString.Add("from", HttpUtility.UrlEncode(Sender));
            wc.QueryString.Add("to", HttpUtility.UrlEncode(to));
            wc.QueryString.Add("text", HttpUtility.UrlEncode(text));
            return ParseSmsResponseJson(wc.DownloadString(""));
        }


        NexmoResponse ParseSmsResponseJson(string json)
        {
            json = json.Replace("-", "");  // hyphens are not allowed in in .NET var names
            return new JavaScriptSerializer().Deserialize<NexmoResponse>(json);
        }
    }
}
