using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using AdventureWorks.Web.Handlers;
using ExceptionHandler = AdventureWorks.Web.Handlers.ExceptionHandler;

namespace AdventureWorks.Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // using Microsoft.Extension.DependencyInjection here.
            Startup.Bootstrapper(config);

            // Web API routes
            config.MapHttpAttributeRoutes();

            Logger.InitLogger();

            config.Services.Replace(typeof(IExceptionHandler), new ExceptionHandler());

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}