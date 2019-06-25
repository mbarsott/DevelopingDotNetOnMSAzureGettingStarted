using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.ProjectOxford.Face;
using Microsoft.ProjectOxford.Face.Contract;
using Microsoft.WindowsAzure.Storage.Blob;

namespace PSHelloFunctions
{
    public static class ImageAnalysis
    {
        [FunctionName("ImageAnalysis")]
        public static async Task Run([BlobTrigger("images/{name}", Connection = "pshellostoragemfb")]
            CloudBlockBlob blob, string name, ILogger log,
            [CosmosDB("pshelloazuredb", "images", ConnectionStringSetting = "psdb")]
            IAsyncCollector<FaceAnalysisResults> result)
        {
            log.LogInformation(
                $"C# Blob trigger function Processed blob\n Name:{blob.Name} \n Size: {blob.Properties.Length} Bytes");
            var sas = GetSas(blob);
            var url = blob.Uri + sas;
            log.LogInformation($"Blob url is {url}");

            var faces = await GetanalysisAsync(url);
            await result.AddAsync(new FaceAnalysisResults {Faces = faces, Id = blob.Name});
        }

        public static async Task<Face[]> GetanalysisAsync(string url)
        {
            var client = new FaceServiceClient(
                "474ef3d083e24ce3869a5177eebff28c",
                "https://eastus.api.cognitive.microsoft.com/face/v1.0");
            var types = new[] {FaceAttributeType.Emotion};
            var result = await client.DetectAsync(url, false, false, types);
            return result;
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