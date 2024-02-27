using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WeatherArchiveApp.Models;
using WeatherArchiveApp.ViewModels;

namespace WeatherArchiveApp.Controllers;

public class WeatherArchiveController : Controller
{
    private readonly ILogger<WeatherArchiveController> _logger;
    private readonly IMonthExtractor<string> _monthExtractor;
    private readonly IYearExtractor<string> _yearExtractor;
    private readonly IPartOfRecordsExtractor _partOfRecordsExtractor;
    private readonly IDateParamsValidator<string> _dateParamsValidator;

    public WeatherArchiveController(ILogger<WeatherArchiveController> logger, IMonthExtractor<string> monthExtractor, IYearExtractor<string> yearExtrcator, IPartOfRecordsExtractor partOfRecordsExtractor, IDateParamsValidator<string> dateParamsValidator)
    {
        _logger = logger;
        _monthExtractor = monthExtractor;
        _yearExtractor = yearExtrcator;
        _partOfRecordsExtractor = partOfRecordsExtractor;
        _dateParamsValidator = dateParamsValidator;
    }

    public IActionResult Uploader()
    {
        return View();
    }

    public IActionResult Viewer(string month, string year)
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            List<WeatherRecord> partOfRecords = new();

            WeatherArchiveViewModel viewModel = new WeatherArchiveViewModel();

            var weatherRecords = db.WeatherRecords.ToList();

            var validationResult = _dateParamsValidator.ValidateDateParams(year, month, weatherRecords);

            switch (validationResult)
            {
                case DateParamsValidationResult.InvalidAll:
                    {
                        viewModel.WeatherRecords = _partOfRecordsExtractor.GetEarlistPartOfRecords(weatherRecords);
                        viewModel.MonthStrings = _monthExtractor.GetMonthsForEarlyYear(weatherRecords);
                        viewModel.SelectedYear = _yearExtractor.GetYears(weatherRecords).First();
                        viewModel.SelectedMonth = _monthExtractor.GetMonthsForEarlyYear(weatherRecords).First();
                        break;
                    }
                case DateParamsValidationResult.ValidAll:
                    {
                        viewModel.WeatherRecords = _partOfRecordsExtractor.GetPartOfRecords(year, month, weatherRecords);
                        viewModel.MonthStrings = _monthExtractor.GetMonths(weatherRecords, year);
                        viewModel.SelectedYear = year;
                        viewModel.SelectedMonth = month;
                        break;
                    }
                case DateParamsValidationResult.ValidYear:
                    {
                        viewModel.WeatherRecords = _partOfRecordsExtractor.GetEarlistMonthPartOfRecords(year, weatherRecords);
                        viewModel.MonthStrings = _monthExtractor.GetMonths(weatherRecords, year);
                        viewModel.SelectedYear = year;
                        viewModel.SelectedMonth = _monthExtractor.GetMonths(weatherRecords, year).First();
                        break;
                    }
            }

            viewModel.YearStrings = _yearExtractor.GetYears(weatherRecords);

            return View(viewModel);
        }
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
