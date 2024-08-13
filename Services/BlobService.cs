
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Collections.Generic;
using System.Threading.Tasks;

public class BlobService
{
    private readonly CloudBlobClient _blobClient;

    public BlobService(string connectionString)
    {
        var storageAccount = CloudStorageAccount.Parse(connectionString);
        _blobClient = storageAccount.CreateCloudBlobClient();
    }

    public async Task<IEnumerable<string>> ListBlobNamesAsync(string containerName)
    {
        var container = _blobClient.GetContainerReference(containerName);
        await container.CreateIfNotExistsAsync();

        var blobNames = new List<string>();

        BlobContinuationToken continuationToken = null;
        do
        {
            var results = await container.ListBlobsSegmentedAsync(null, true, BlobListingDetails.None, null, continuationToken, null, null);
            continuationToken = results.ContinuationToken;

            foreach (var item in results.Results)
            {
                if (item is CloudBlob blob)
                {
                    blobNames.Add(blob.Name);
                }
            }
        } while (continuationToken != null);

        return blobNames;
    }

    public async Task<CloudBlockBlob> GetBlobAsync(string containerName, string blobName)
    {
        var container = _blobClient.GetContainerReference(containerName);
        var blob = container.GetBlockBlobReference(blobName);
        return blob;
    }

    public string GetBlobUrl(string containerName, string fileName)
    {
        var container = _blobClient.GetContainerReference(containerName);
        var blob = container.GetBlockBlobReference(fileName);

        return blob.Uri.ToString();
    }
}
