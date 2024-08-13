using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using System.Threading.Tasks;

public class QueueService
{
    private readonly CloudQueueClient _queueClient;

    public QueueService(string connectionString)
    {
        var storageAccount = CloudStorageAccount.Parse(connectionString);
        _queueClient = storageAccount.CreateCloudQueueClient();
    }

    public async Task AddMessageAsync(string queueName, string message)
    {
        var queue = _queueClient.GetQueueReference(queueName);
        await queue.CreateIfNotExistsAsync();
        var cloudMessage = new CloudQueueMessage(message);
        await queue.AddMessageAsync(cloudMessage);
    }

    public async Task<CloudQueueMessage> GetMessageAsync(string queueName)
    {
        var queue = _queueClient.GetQueueReference(queueName);
        await queue.CreateIfNotExistsAsync();
        var message = await queue.GetMessageAsync();
        return message;
    }
}
