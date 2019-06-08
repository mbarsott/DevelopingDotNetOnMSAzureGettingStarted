using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.IO;
using System.Threading.Tasks;

namespace PsHelloAzure.Services
{
    public class ImageStore
    {
        private CloudBlobClient _blobClient;
        private string _baseUri = "https://pshellostoragemfb.blob.core.windows.net/";

        public ImageStore()
        {
            var credentials = new StorageCredentials("pshellostoragemfb",
                "k4PWITMDTXn9IYGhIHOGvkN4Jcof6kkK/lO+t8Tz8oa4myyTzLFPVdQmoqoiaqKi7opsLUh7E/X/Q/f/jIU7wA==");
            _blobClient = new CloudBlobClient(new Uri(_baseUri), credentials);
        }

        public async Task<string> SaveImage(Stream imageStream)
        {
            var imageId = Guid.NewGuid().ToString();
            var container = _blobClient.GetContainerReference("images");
            var blob= container.GetBlockBlobReference(imageId);
            await blob.UploadFromStreamAsync(imageStream);

            return imageId;
        }

        public string UriFor(string imageId)
        {
            return $"{_baseUri}images/{imageId}";
        }
    }
}
