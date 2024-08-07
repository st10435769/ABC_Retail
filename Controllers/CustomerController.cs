using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.Azure.Cosmos.Table;
using WebApplication1.Models;


public class CustomerController : Controller
{
    private readonly AzureTableStorageRepository _repository = new AzureTableStorageRepository();

    public async Task<ActionResult> Index()
    {
        var table = await _repository.GetTableAsync("Customers");
        var query = new TableQuery<CustomerEntity>();
        var customers = new List<CustomerEntity>();

        TableContinuationToken token = null;
        do
        {
            var queryResult = await table.ExecuteQuerySegmentedAsync(query, token);
            customers.AddRange(queryResult.Results);
            token = queryResult.ContinuationToken;
        } while (token != null);

        return View(customers);
    }

    [HttpGet]
    public ActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<ActionResult> Create(CustomerEntity customer)
    {
        var table = await _repository.GetTableAsync("Customers");
        await _repository.InsertOrMergeEntityAsync(table, customer);
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<ActionResult> Delete(string partitionKey, string rowKey)
    {
        var table = await _repository.GetTableAsync("Customers");
        var retrieveResult = await _repository.RetrieveEntityAsync<CustomerEntity>(table, partitionKey, rowKey);
        if (retrieveResult.Result is CustomerEntity customer)
        {
            await _repository.DeleteEntityAsync(table, customer);
        }
        return RedirectToAction("Index");
    }
}
