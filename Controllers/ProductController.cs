using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.Azure.Cosmos.Table;
using WebApplication1.Models;


public class ProductController : Controller
{
    private readonly AzureTableStorageRepository _repository = new AzureTableStorageRepository();

    public async Task<ActionResult> Index()
    {
        var table = await _repository.GetTableAsync("Products");
        var query = new TableQuery<ProductEntity>();
        var products = new List<ProductEntity>();

        TableContinuationToken token = null;
        do
        {
            var queryResult = await table.ExecuteQuerySegmentedAsync(query, token);
            products.AddRange(queryResult.Results);
            token = queryResult.ContinuationToken;
        } while (token != null);

        return View(products);
    }

    [HttpGet]
    public ActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<ActionResult> Create(ProductEntity product)
    {
        var table = await _repository.GetTableAsync("Products");
        await _repository.InsertOrMergeEntityAsync(table, product);
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<ActionResult> Delete(string partitionKey, string rowKey)
    {
        var table = await _repository.GetTableAsync("Products");
        var retrieveResult = await _repository.RetrieveEntityAsync<ProductEntity>(table, partitionKey, rowKey);
        if (retrieveResult.Result is ProductEntity product)
        {
            await _repository.DeleteEntityAsync(table, product);
        }
        return RedirectToAction("Index");
    }
}
