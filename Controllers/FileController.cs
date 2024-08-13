using Microsoft.Azure.Cosmos.Table;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using System.Web.ApplicationServices;
using System.Web.Mvc;

public class FileController : Controller
{
    private readonly FileService _fileService;

    public FileController()
    {
        var connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["BlobStorageConnectionString"].ConnectionString;
        _fileService = new FileService(connectionString);
    }

    public ActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<ActionResult> Upload(HttpPostedFileBase file)
    {
        if (file != null && file.ContentLength > 0)
        {
            var fileName = Path.GetFileName(file.FileName);
            await _fileService.UploadFileAsync("contracts-share", "contracts", fileName, file.InputStream);
            ViewBag.Message = $"File {fileName} uploaded successfully.";
        }
        return View("Index");
    }

    public async Task<ActionResult> Download(string fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName))
        {
            ViewBag.Message = "File name cannot be empty.";
            return View("Index");
        }

        try
        {
            var file = await _fileService.GetFileAsync("contracts-share", "contracts", fileName);
            var fileStream = await file.OpenReadAsync();
            ViewBag.Message = null; // Clear any previous error messages
            return File(fileStream, "application/octet-stream", fileName);
        }
        catch (StorageException ex) when (ex.RequestInformation.HttpStatusCode == 404)
        {
            ViewBag.Message = "Error: File not found.";
            return View("Index");
        }
        catch (Exception ex)
        {
            ViewBag.Message = $"Error: {ex.Message}";
            return View("Index");
        }
    }
}
