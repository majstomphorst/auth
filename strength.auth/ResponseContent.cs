using Newtonsoft.Json;

namespace strength.auth;



public class ResponseContent
{
    public static class Actions
    {
        public static string ValidationError = "ValidationError";
        public static string ShowBlockPage = "ShowBlockPage";
        public static string Continue = "Continue";
    }

    public const string ApiVersion = "1.0.0";

    public ResponseContent()
    {
        this.version = ResponseContent.ApiVersion;
        this.action = "Continue";
    }

    public ResponseContent(string action, string userMessage)
    {
        this.version = ResponseContent.ApiVersion;
        this.action = action;
        this.userMessage = userMessage;
        if (action == "ValidationError")
        {
            this.status = "400";
        }
    }

    public string version { get; }
    public string action { get; set; }


    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string userMessage { get; set; }


    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string status { get; set; }


    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string jobTitle { get; set; }

    //[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    //public string extension_CustomClaim { get; set; }
}