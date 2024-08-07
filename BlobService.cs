using Azure.Storage.Blobs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace WebApplication1
{
    public class BlobService
    {
        private readonly BlobServiceClient _blobServiceClient;

        public BlobService(string connectionString)
        {
            _blobServiceClient = new BlobServiceClient(connectionString);
        }

        public async Task UploadFileAsync(string containerName, string filePath)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            await containerClient.CreateIfNotExistsAsync();

            var blobClient = containerClient.GetBlobClient(Path.GetFileName(filePath));

            // Using traditional `using` block to ensure compatibility with C# 7.3
            using (FileStream uploadFileStream = File.OpenRead(filePath))
            {
                await blobClient.UploadAsync(uploadFileStream, true);
            }
        }

        internal string GetBlobUrl(string v, string fileName)
        {
            throw new NotImplementedException();
        }
    }
}