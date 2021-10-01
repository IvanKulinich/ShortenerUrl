using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ShortenerUrl.Interfaces;
using ShortenerUrl.Models;
using System;
using System.Diagnostics;

namespace ShortenerUrl.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IShortenerUrlService _shortenerUrlService;

        public HomeController(
            ILogger<HomeController> logger,
            IShortenerUrlService shortenerUrlService)
        {
            _logger = logger;
            _shortenerUrlService = shortenerUrlService;
        }

        public IActionResult Index()
        {
            var result = _shortenerUrlService.GetAllItemsAsync()
                .GetAwaiter()
                .GetResult();

            return View(result);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }

        [HttpPost]
        [Route("/")]
        public IActionResult PostURL([FromBody] UrlRequestModel model)
        {
            try
            {
                Uri uriResult;
                bool isValidUrl = Uri.TryCreate(model.Url, UriKind.Absolute, out uriResult)
                    && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

                if (!isValidUrl)
                {
                    throw new Exception("This is not url.");
                }

                var result = _shortenerUrlService.CreateShortenedUrlAsync(uriResult.AbsoluteUri.Trim('/'))
                    .GetAwaiter()
                    .GetResult();

                return Json(result);
            }
            catch (Exception ex)
            {
                if (ex.Message == "URL already exists")
                {
                    return BadRequest(new UrlResponse()
                    {
                        Url = model.Url,
                        Status = ex.Message,
                        ShortedUrl = _shortenerUrlService.GetShortenedUrlByRealUrlAsync(model.Url.Trim('/')).GetAwaiter().GetResult()
                    });
                }

                return BadRequest(ex.Message);
            }
        }

        [HttpGet, Route("/{token}")]
        public IActionResult RedirectToRealUrl([FromRoute] string token)
        {
            try
            {
                var realUrl = _shortenerUrlService.GetRealUrlByTokenAsync(token)
                    .GetAwaiter()
                    .GetResult();

                if (realUrl == null)
                {
                    throw new Exception("Real url not found.");
                }

                return Redirect(realUrl);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
