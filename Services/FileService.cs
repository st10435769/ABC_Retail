using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.File;
using System.IO;
using System.Threading.Tasks;

public class FileService
{
    private readonly CloudFileClient _fileClient;

    public FileService(string connectionString)
    {
        var storageAccount = CloudStorageAccount.Parse(connectionString);
        _fileClient = storageAccount.CreateCloudFileClient();
    }

    public async Task UploadFileAsync(string shareName, string directoryName, string fileName, Stream fileStream)
    {
        var share = _fileClient.GetShareReference(shareName);
        await share.CreateIfNotExistsAsync();

        var rootDir = share.GetRootDirectoryReference();
        var directory = rootDir.GetDirectoryReference(directoryName);
        await directory.CreateIfNotExistsAsync();

        var file = directory.GetFileReference(fileName);
        await file.UploadFromStreamAsync(fileStream);
    }

    public async Task<CloudFile> GetFileAsync(string shareName, string directoryName, string fileName)
    {
        var share = _fileClient.GetShareReference(shareName);
        var rootDir = share.GetRootDirectoryReference();
        var directory = rootDir.GetDirectoryReference(directoryName);

        var file = directory.GetFileReference(fileName);
        return file;
    }
}
