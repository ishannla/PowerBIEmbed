using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace PowerBIEmbedWeb
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                // these parameter names have to match method parameter names
                routeTemplate: "api/{controller}/{action}/{parameter1}/{parameter2}/{parameter3}",
                    defaults: new
                    {
                        parameter1 = RouteParameter.Optional,
                        parameter2 = RouteParameter.Optional,
                        parameter3 = RouteParameter.Optional
                    }
            );
        }
    }
}
