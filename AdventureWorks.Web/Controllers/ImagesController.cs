using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using AdventureWorks.Services.Images;
using AdventureWorks.Web.Filters;
using AdventureWorks.Web.Models;

namespace AdventureWorks.Web.Controllers
{
    public class ImagesController : ApiController
    {
        private string _serverUploadFolder;
        private readonly IFileStore _fileStore;

        public ImagesController(
            IFileStore fileStore)
        {
            _fileStore = fileStore;
            _serverUploadFolder = Path.Combine(Path.GetTempPath(), "Files");
            if (!Directory.Exists(_serverUploadFolder))
            {
                Directory.CreateDirectory(_serverUploadFolder);
            }
        }

        [ValidateMimeMultipartContentFilter]
        [HttpPost, Route("upload")]
        public async Task<SoftwarePackageModel> UploadSingleFile()
        {
            var streamProvider = new MultipartFormDataStreamProvider(_serverUploadFolder);
            var res = await Request.Content.ReadAsMultipartAsync(streamProvider).ContinueWith(async t =>
            {
                var firstFile = streamProvider.FileData.FirstOrDefault();

                string uri = null;
                if (firstFile != null)
                {
                    using (var fs = new FileStream(firstFile.LocalFileName, FileMode.Open))
                    {
                        var fileName = firstFile.Headers.ContentDisposition.FileName.Trim('"');
                        uri = await _fileStore.Save(fileName, fs);
                    }
                }

                return new SoftwarePackageModel
                {
                    Uri = uri
                };
            });

            return await res;
        }
    }
}