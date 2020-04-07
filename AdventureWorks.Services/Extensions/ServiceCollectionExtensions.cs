using AdventureWorks.Services.Images;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.WindowsAzure.Storage;

namespace AdventureWorks.Services.Extensions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds Core services to the container.
        /// </summary>
        public static IServiceCollection AddServices(
            this IServiceCollection services,
            string azureConfigurationString,
            string blobContainerName,
            string queueName)
        {
            services.AddScoped<IFileStore>(
                c =>
                {
                    var account = CloudStorageAccount.Parse(azureConfigurationString);

                    var blobClient = account.CreateCloudBlobClient();
                    var cloudBlobContainer = blobClient.GetContainerReference(blobContainerName.ToLower());
                    cloudBlobContainer.CreateIfNotExists();

                    var queueClient = account.CreateCloudQueueClient();
                    var queue = queueClient.GetQueueReference(queueName.ToLower());
                    queue.CreateIfNotExists();

                    return new FileStore(blobClient, cloudBlobContainer, queue);
                });

            return services;
        }
    }
}