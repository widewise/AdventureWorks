using System;
using System.Configuration;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Controllers;
using AdventureWorks.Services.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace AdventureWorks.Web
{
    public class Startup
    {
        public static void Bootstrapper(HttpConfiguration config)
        {
            var provider = Configuration();
            var resolver = new DefaultDependencyResolver(provider);

            config.DependencyResolver = resolver;
        }

        private static IServiceProvider Configuration()
        {
            var services = new ServiceCollection();

            services.AddControllersAsServices(typeof(Startup).Assembly.GetExportedTypes()
                .Where(t => !t.IsAbstract && !t.IsGenericTypeDefinition)
                .Where(t => typeof(IHttpController).IsAssignableFrom(t)
                            || t.Name.EndsWith("Controller", StringComparison.OrdinalIgnoreCase)));

            var azureConfigurationString = ConfigurationManager.ConnectionStrings["AzureConfigurationString"].ConnectionString;
            var blobContainerName = ConfigurationManager.AppSettings["BlobContainerName"];
            var queueName = ConfigurationManager.AppSettings["QueueName"];

            services.AddServices(azureConfigurationString, blobContainerName, queueName);

            var serviceProvider = services.BuildServiceProvider();

            return serviceProvider;
        }
    }
}