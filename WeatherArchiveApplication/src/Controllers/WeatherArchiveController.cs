using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using src.Models;

namespace src.Controllers;

public class WeatherArchiveController : Controller
{
    private readonly ILogger<WeatherArchiveController> _logger;

    public WeatherArchiveController(ILogger<WeatherArchiveController> logger)
    {
        _logger = logger;
    }

    public IActionResult Uploader()
    {
        return View();
    }

    public IActionResult Viewer()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
