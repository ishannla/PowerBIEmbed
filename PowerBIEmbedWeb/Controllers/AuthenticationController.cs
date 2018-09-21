using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.PowerBI.Api.V2;
using Microsoft.PowerBI.Api.V2.Models;
using Microsoft.Rest;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PowerBIEmbedWeb.Controllers
{
    public class AuthenticationController : ApiController
    {
        public PowerBIClient client;

        [HttpGet] 
        // parameter options: "generate_new_token" or "use_cached_token"
        // parameter indicates whether user should be prompted for credentials or to check cache for current token session
        public string GetAccessToken(string parameter1)
        {
            var desiredAction = parameter1;
            var authContext = new AuthenticationContext(Secret.aadAuthorizationEndpoint);

            try
            {
                PlatformParameters platformParameters = null;

                if (desiredAction.Equals("generate_new_token"))
                    platformParameters = new PlatformParameters(PromptBehavior.SelectAccount);
                else if (desiredAction.Equals("use_cached_token"))
                    platformParameters = new PlatformParameters(PromptBehavior.Auto);

                // TODO caching and stuff confirmed working, so uncomment next 4 lines and delete username/password auth
                var userAuthResult = authContext.AcquireTokenAsync(Secret.pbiLoginResourceUrl,
                                                             Secret.clientId,
                                                             new Uri(Secret.redirectUrl),
                                                             platformParameters).Result;
                //var userAuthResult = authContext.AcquireTokenAsync(Secret.pbiLoginResourceUrl,
                //                                               Secret.clientId,
                //                                               new UserPasswordCredential("user@pbiembd.onmicrosoft.com",
                //                                                               "wallet79*")).Result;

                var tokenCredentials = new TokenCredentials(userAuthResult.AccessToken, "Bearer");
                client = new PowerBIClient(new Uri(Secret.apiUrl), tokenCredentials);

                return userAuthResult.AccessToken;
            }

            catch (System.AggregateException)
            {
                return "user_cancelled_login";
            }
        }

        
        [HttpGet]
        public IList<Group> GetWorkspaces()
        {
            // re-initializes client if window location been changed
            if (client == null)
                GetAccessToken("use_cached_token");

            // assumes that client has been initialized
            return client.Groups.GetGroups().Value;
        }


        [HttpGet]
        // parameter options: "myworkspace" OR whatever the id of the workspace is
        public IList<Report> GetReportsFromGroup(string parameter1)
        {
            var workspaceId = parameter1;

            // re-initializes client if window location been changed
            if (client == null)
                GetAccessToken("use_cached_token");

            // assumes that client has been initialized
            if (!workspaceId.Equals("myworkspace"))
                return client.Reports.GetReportsInGroup(workspaceId).Value;
            // need to get all reports and remove any of the reports contained in an external workspace
            else
            {
                IList<Group> groups = client.Groups.GetGroups().Value;

                // also contains any report owned by a global admin within same domain
                IList<Report> allReports = client.Reports.GetReports().Value;
                HashSet<String> reportIds = new HashSet<String>();
                IList<Report> myWorkspaceReports = new List<Report>();

                foreach (Report r in allReports)
                    reportIds.Add(r.Id);

                // removing all reportIds not contained in my workspace
                foreach (Group g in groups)
                {
                    IList<Report> reportsInGroup = client.Reports.GetReportsInGroup(g.Id).Value;

                    foreach (Report r in reportsInGroup)
                        reportIds.Remove(r.Id);
                }

                // removing all reports not contained within reportIds
                foreach (Report r in allReports)
                {
                    if (reportIds.Contains(r.Id))
                        myWorkspaceReports.Add(r);
                }

                return myWorkspaceReports;
            }
        }


        [HttpGet]
        // parameter1 represents workspaceId, parameter2 represents reportId
        // parameter3 options: "view" or "edit"
        public string GetEmbedToken(string parameter1, string parameter2, string parameter3)
        {
            var workspaceId = parameter1;
            var reportId = parameter2;
            var permission = parameter3;

            // re-initializes client if window location been changed
            if (client == null)
                GetAccessToken("use_cached_token");

            var report = client.Reports.GetReportInGroup(workspaceId, reportId);

            GenerateTokenRequest generateTokenRequestParameters = new GenerateTokenRequest(accessLevel: permission);
            string embedToken = client.Reports.GenerateTokenInGroup(workspaceId, report.Id, generateTokenRequestParameters).Token;
            return embedToken;
        }

    }
}
