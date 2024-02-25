using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WeatherArchiveApp.Models;

namespace WeatherArchiveApp.Controllers;

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
        using (ApplicationContext db = new ApplicationContext())
        {
            var weatherRecords = db.WeatherRecords.ToList();
            return View(weatherRecords);
        }
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
