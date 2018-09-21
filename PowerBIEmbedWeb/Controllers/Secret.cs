using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PowerBIEmbedWeb.Controllers
{
    public class Secret
    {
        public static string clientId = "CLIENT_ID";
        public static string redirectUrl = "REDIRECT_URL";

        public static string aadAuthorizationEndpoint = "https://login.windows.net/common/oauth2/authorize";
        public static string pbiLoginResourceUrl = "https://analysis.windows.net/powerbi/api";
        public static string apiUrl = "https://api.powerbi.com/";
    }
}