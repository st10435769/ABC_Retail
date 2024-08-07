using System.Threading.Tasks;
using System.Web.Mvc;

public class ImagesController : Controller
{
    private readonly BlobService _blobService;

    public ImagesController()
    {
        var connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["BlobStorageConnectionString"].ConnectionString;
        _blobService = new BlobService(connectionString);
    }

    // Action to display the list of images
    public async Task<ActionResult> Index()
    {
        var fileNames = await _blobService.ListBlobNamesAsync("media");
        return View(fileNames);
    }

    // Action to serve the image
    public async Task<ActionResult> ShowImage(string fileName)
    {
        var blob = await _blobService.GetBlobAsync("media", fileName);

        if (blob != null)
        {
            var stream = await blob.OpenReadAsync();
            var contentType = blob.Properties.ContentType;
            return File(stream, contentType);
        }

        return HttpNotFound();
    }

}
