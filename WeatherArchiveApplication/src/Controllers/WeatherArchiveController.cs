using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WeatherArchiveApp.Models;
using WeatherArchiveApp.ViewModels;

namespace WeatherArchiveApp.Controllers;

public class WeatherArchiveController : Controller
{
    private readonly ILogger<WeatherArchiveController> _logger;
    private readonly IMonthExtractor _monthExtractor;
    private readonly IYearExtractor _yearExtractor;

    public WeatherArchiveController(ILogger<WeatherArchiveController> logger, IMonthExtractor monthExtractor, IYearExtractor yearExtrcator)
    {
        _logger = logger;
        _monthExtractor = monthExtractor;
        _yearExtractor = yearExtrcator;
    }

    public IActionResult Uploader()
    {
        return View();
    }

    public IActionResult Viewer()
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            WeatherArchiveViewModel viewModel = new WeatherArchiveViewModel();

            var weatherRecords = db.WeatherRecords.ToList();

            viewModel.WeatherRecords = weatherRecords;
            viewModel.MonthStrings = _monthExtractor.GetMonthStrings(weatherRecords);
            viewModel.YearStrings = _yearExtractor.GetYearStrings(weatherRecords);

            return View(viewModel);
        }
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
