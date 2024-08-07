using Microsoft.Azure.Cosmos.Table;
using System.Configuration;
using System.Threading.Tasks;

public class AzureTableStorageRepository
{
    private CloudTableClient tableClient;

    public AzureTableStorageRepository()
    {
        var storageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["StorageConnectionString"]);
        tableClient = storageAccount.CreateCloudTableClient(new TableClientConfiguration());
    }

    public async Task<CloudTable> GetTableAsync(string tableName)
    {
        var table = tableClient.GetTableReference(tableName);
        await table.CreateIfNotExistsAsync();
        return table;
    }

    public async Task InsertOrMergeEntityAsync<T>(CloudTable table, T entity) where T : ITableEntity
    {
        var insertOrMergeOperation = TableOperation.InsertOrMerge(entity);
        await table.ExecuteAsync(insertOrMergeOperation);
    }

    public async Task<TableResult> RetrieveEntityAsync<T>(CloudTable table, string partitionKey, string rowKey) where T : ITableEntity
    {
        var retrieveOperation = TableOperation.Retrieve<T>(partitionKey, rowKey);
        return await table.ExecuteAsync(retrieveOperation);
    }

    public async Task DeleteEntityAsync<T>(CloudTable table, T entity) where T : ITableEntity
    {
        var deleteOperation = TableOperation.Delete(entity);
        await table.ExecuteAsync(deleteOperation);
    }
}
