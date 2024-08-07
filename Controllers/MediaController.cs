using System.Threading.Tasks;
using System.Web.Mvc;

public class MediaController : Controller
{
    private readonly BlobService _blobService;

    public MediaController()
    {
        var connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["BlobStorageConnectionString"].ConnectionString;
        _blobService = new BlobService(connectionString);
    }

    public async Task<ActionResult> ShowImage(string fileName)
    {
        var blobUrl = _blobService.GetBlobUrl("media", fileName);
        return Redirect(blobUrl); // Redirect to the blob URL
    }
}
