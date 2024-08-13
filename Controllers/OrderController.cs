using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.WindowsAzure.Storage.Queue;

public class OrderController : Controller
{
    private readonly QueueService _queueService;

    public OrderController()
    {
        var connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["BlobStorageConnectionString"].ConnectionString;
        _queueService = new QueueService(connectionString);
    }

    public ActionResult Index()
    {
        return View();
    }

    public async Task<ActionResult> ProcessOrder(string orderId)
    {
        await _queueService.AddMessageAsync("order-queue", $"Processing order {orderId}");
        ViewBag.Message = $"Order {orderId} is being processed.";
        return View("Index");
    }

    public async Task<ActionResult> RetrieveOrderMessage()
    {
        var message = await _queueService.GetMessageAsync("order-queue");
        if (message != null)
        {
            ViewBag.Message = $"Retrieved message: {message.AsString}";
        }
        else
        {
            ViewBag.Message = "No messages found in the queue.";
        }
        return View("RetrieveOrderMessage");
    }
}
