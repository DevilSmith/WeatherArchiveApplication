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
    private readonly IFileExtensionValidator<IFormFile, bool> _fileExtensionValidator;
    private readonly IFileDataValidator<IFormFile> _fileDataValidator;

    public WeatherArchiveController(
        ILogger<WeatherArchiveController> logger,
        IMonthExtractor<string> monthExtractor,
        IYearExtractor<string> yearExtrcator,
        IPartOfRecordsExtractor partOfRecordsExtractor,
        IDateParamsValidator<string> dateParamsValidator,
        IFileExtensionValidator<IFormFile, bool> fileExtensionValidator,
        IFileDataValidator<IFormFile> fileDataValidator)
    {
        _logger = logger;
        _monthExtractor = monthExtractor;
        _yearExtractor = yearExtrcator;
        _partOfRecordsExtractor = partOfRecordsExtractor;
        _dateParamsValidator = dateParamsValidator;
        _fileExtensionValidator = fileExtensionValidator;
        _fileDataValidator = fileDataValidator;
    }

    [HttpPost]
    [RequestSizeLimit(512 * 1024 * 1024)]
    public IActionResult MultiFileUpload(UploaderFilesViewModel uploaderFilesViewModel)
    {
        if (uploaderFilesViewModel is not null)
        {
            if (ModelState.IsValid)
            {
                uploaderFilesViewModel.IsResponse = true;

                if (uploaderFilesViewModel.Files.Count > 0)
                {

                    foreach (var file in uploaderFilesViewModel.Files)
                    {
                        if (_fileExtensionValidator.ValidateFileExtension(file) == false)
                        {
                            uploaderFilesViewModel.IsSuccess = false;
                            uploaderFilesViewModel.Message = "File extension must be .xlsx. Please, select another files.";
                            return View("Uploader", uploaderFilesViewModel);
                        }

                        _fileDataValidator.ValidateFileData(file);

                    }

                    uploaderFilesViewModel.IsSuccess = true;
                    uploaderFilesViewModel.Message = "Files upload successfully";
                    return View("Uploader", uploaderFilesViewModel);
                }
                else
                {
                    uploaderFilesViewModel.IsSuccess = false;
                    uploaderFilesViewModel.Message = "Please select files";
                    return View("Uploader", uploaderFilesViewModel);
                }
            }
        }

        UploaderFilesViewModel errorViewModel = new();
        errorViewModel.IsSuccess = false;
        errorViewModel.Message = "File is incorrect";
        errorViewModel.Files = new();

        return View("Uploader", errorViewModel);
    }

    public IActionResult Uploader()
    {
        UploaderFilesViewModel viewModel = new UploaderFilesViewModel();

        return View(viewModel);
    }

    public IActionResult Viewer(string month, string year)
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            List<WeatherRecord> partOfRecords = new();

            TableOfRecordsViewModel tableOfRecordsViewModel = new TableOfRecordsViewModel();

            var weatherRecords = db.WeatherRecords.ToList();

            if (weatherRecords.Count == 0) return View("EmptyData");

            var validationResult = _dateParamsValidator.ValidateDateParams(year, month, weatherRecords);

            switch (validationResult)
            {
                case DateParamsValidationResult.InvalidAll:
                    {
                        tableOfRecordsViewModel.SelectedYear = _yearExtractor.GetYears(weatherRecords).First();
                        tableOfRecordsViewModel.SelectedMonth = _monthExtractor.GetMonths(weatherRecords, tableOfRecordsViewModel.SelectedYear).First();
                        tableOfRecordsViewModel.WeatherRecords = _partOfRecordsExtractor.GetPartOfRecords(tableOfRecordsViewModel.SelectedYear, tableOfRecordsViewModel.SelectedMonth, weatherRecords);
                        tableOfRecordsViewModel.MonthStrings = _monthExtractor.GetMonths(weatherRecords, tableOfRecordsViewModel.SelectedYear);
                        break;
                    }
                case DateParamsValidationResult.ValidAll:
                    {
                        tableOfRecordsViewModel.SelectedYear = year;
                        tableOfRecordsViewModel.SelectedMonth = month;
                        tableOfRecordsViewModel.WeatherRecords = _partOfRecordsExtractor.GetPartOfRecords(year, month, weatherRecords);
                        tableOfRecordsViewModel.MonthStrings = _monthExtractor.GetMonths(weatherRecords, year);
                        break;
                    }
                case DateParamsValidationResult.ValidYear:
                    {
                        tableOfRecordsViewModel.SelectedYear = year;
                        tableOfRecordsViewModel.SelectedMonth = _monthExtractor.GetMonths(weatherRecords, year).First();
                        tableOfRecordsViewModel.WeatherRecords = _partOfRecordsExtractor.GetPartOfRecords(year, tableOfRecordsViewModel.SelectedMonth, weatherRecords);
                        tableOfRecordsViewModel.MonthStrings = _monthExtractor.GetMonths(weatherRecords, year);
                        break;
                    }
            }

            tableOfRecordsViewModel.YearStrings = _yearExtractor.GetYears(weatherRecords);

            return View(tableOfRecordsViewModel);
        }
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
