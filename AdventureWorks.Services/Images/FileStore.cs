using System.IO;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;

namespace AdventureWorks.Services.Images
{
    public class FileStore : IFileStore
    {
        private CloudBlobClient _blobClient;
        private readonly CloudBlobContainer _filesBlobContainer;
        private readonly CloudQueue _queue;

        public FileStore(
            CloudBlobClient blobClient,
            CloudBlobContainer filesBlobContainer,
            CloudQueue queue)
        {
            _blobClient = blobClient;
            _filesBlobContainer = filesBlobContainer;
            _queue = queue;
        }

        public async Task<string> Save(string fileName, Stream fileStream)
        {
            var fileBlob = _filesBlobContainer.GetBlockBlobReference(fileName);

            await fileBlob.UploadFromStreamAsync(fileStream);
            var azureFileName = $"{_blobClient.BaseUri}{_filesBlobContainer.Name}/{fileName}";

            var notification = FileNotificationFactory.Create(fileName, azureFileName);

            var serializer = new JsonSerializer();
            var stringWriter = new StringWriter();
            serializer.Serialize(stringWriter, notification);

            CloudQueueMessage message = new CloudQueueMessage(stringWriter.ToString());
            await _queue.AddMessageAsync(message);

            return azureFileName;
        }
    }
}