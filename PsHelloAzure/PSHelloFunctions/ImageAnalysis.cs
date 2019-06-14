using System;
using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Blob;

namespace PSHelloFunctions
{
    public static class ImageAnalysis
    {
        [FunctionName("ImageAnalysis")]
        public static void Run([BlobTrigger("images/{name}", Connection = "pshellostoragemfb")] CloudBlockBlob blob, string name, ILogger log)
        {
            log.LogInformation($"C# Blob trigger function Processed blob\n Name:{blob.Name} \n Size: {blob.Properties.Length} Bytes");
            var sas = GetSas(blob);
            var url = blob.Uri + sas;
            log.LogInformation($"Blob url is {url}");
        }

        public static string GetSas(CloudBlockBlob blob)
        {
            var sasPolicy = new SharedAccessBlobPolicy
            {
                Permissions = SharedAccessBlobPermissions.Read,
                SharedAccessStartTime = DateTime.UtcNow.AddMinutes(-15),
                SharedAccessExpiryTime = DateTime.UtcNow.AddMinutes(15)
            };
            var sas = blob.GetSharedAccessSignature(sasPolicy);
            return sas;
        }
    }
}
