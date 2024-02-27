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

            if (weatherRecords.Count == 0) return View("EmptyData");

            var validationResult = _dateParamsValidator.ValidateDateParams(year, month, weatherRecords);

            switch (validationResult)
            {
                case DateParamsValidationResult.InvalidAll:
                    {
                        viewModel.SelectedYear = _yearExtractor.GetYears(weatherRecords).First();
                        viewModel.SelectedMonth = _monthExtractor.GetMonths(weatherRecords, viewModel.SelectedYear).First();
                        viewModel.WeatherRecords = _partOfRecordsExtractor.GetPartOfRecords(viewModel.SelectedYear, viewModel.SelectedMonth, weatherRecords);
                        viewModel.MonthStrings = _monthExtractor.GetMonths(weatherRecords, viewModel.SelectedYear);
                        break;
                    }
                case DateParamsValidationResult.ValidAll:
                    {
                        viewModel.SelectedYear = year;
                        viewModel.SelectedMonth = month;
                        viewModel.WeatherRecords = _partOfRecordsExtractor.GetPartOfRecords(year, month, weatherRecords);
                        viewModel.MonthStrings = _monthExtractor.GetMonths(weatherRecords, year);
                        break;
                    }
                case DateParamsValidationResult.ValidYear:
                    {
                        viewModel.SelectedYear = year;
                        viewModel.SelectedMonth = _monthExtractor.GetMonths(weatherRecords, year).First();
                        viewModel.WeatherRecords = _partOfRecordsExtractor.GetPartOfRecords(year, viewModel.SelectedMonth, weatherRecords);
                        viewModel.MonthStrings = _monthExtractor.GetMonths(weatherRecords, year);
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
