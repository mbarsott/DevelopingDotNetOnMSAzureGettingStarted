using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PsHelloAzure.Services;
using System.Threading.Tasks;
using PsHelloAzure.Models;

namespace PsHelloAzure.Controllers
{
    [Route("[controller]/[action]")]
    public class ImagesController : Controller
    {
        private readonly ImageStore _imageStore;

        public ImagesController(ImageStore imageStore)
        {
            this._imageStore = imageStore;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload(IFormFile image)
        {
            if (image != null)
            {
                using (var stream = image.OpenReadStream())
                {
                    var imageId = await _imageStore.SaveImage(stream);
                    return RedirectToAction("Show", new { imageId });
                }
            }
            return View("Index");
        }

        [HttpGet("{imageId}")]
        public ActionResult Show(string imageId)
        {
            var model = new ShowModel { Uri = _imageStore.UriFor(imageId) };
            return View(model);
        }
    }
}
