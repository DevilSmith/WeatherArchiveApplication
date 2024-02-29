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
    private readonly IDataExtractor<List<WeatherRecord>, IFormFile> _dataExtractor;

    public WeatherArchiveController(
        ILogger<WeatherArchiveController> logger,
        IMonthExtractor<string> monthExtractor,
        IYearExtractor<string> yearExtrcator,
        IPartOfRecordsExtractor partOfRecordsExtractor,
        IDateParamsValidator<string> dateParamsValidator,
        IFileExtensionValidator<IFormFile, bool> fileExtensionValidator,
        IFileDataValidator<IFormFile> fileDataValidator,
        IDataExtractor<List<WeatherRecord>, IFormFile> dataExtractor)
    {
        _logger = logger;
        _monthExtractor = monthExtractor;
        _yearExtractor = yearExtrcator;
        _partOfRecordsExtractor = partOfRecordsExtractor;
        _dateParamsValidator = dateParamsValidator;
        _fileExtensionValidator = fileExtensionValidator;
        _fileDataValidator = fileDataValidator;
        _dataExtractor = dataExtractor;
    }

    delegate void SetupUploaderFilesViewModel(UploaderFilesViewModel viewModel, bool isSuccess, string message);

    SetupUploaderFilesViewModel setupUploaderFilesViewModel = delegate (UploaderFilesViewModel viewModel, bool isSuccess, string message)
    {
        viewModel.IsSuccess = isSuccess;
        viewModel.Message = message;
    };

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
                            setupUploaderFilesViewModel(uploaderFilesViewModel, false, "File extension must be .xlsx. Please, select another files.");
                            return View("Uploader", uploaderFilesViewModel);
                        }

                        if (_fileDataValidator.ValidateFileData(file) == false)
                        {
                            setupUploaderFilesViewModel(uploaderFilesViewModel, false, "File have incorrect structure. Please, select another files.");
                            return View("Uploader", uploaderFilesViewModel);
                        }

                        using (ApplicationContext db = new ApplicationContext())
                        {
                            var extractedData = _dataExtractor.ExtractData(file);

                            foreach (var record in extractedData)
                            {
                                db.WeatherRecords.Add(record);
                            }

                            db.SaveChanges();
                        }

                    }

                    setupUploaderFilesViewModel(uploaderFilesViewModel, true, "Files upload successfully");
                    return View("Uploader", uploaderFilesViewModel);
                }
                else
                {
                    setupUploaderFilesViewModel(uploaderFilesViewModel, false, "Please select files");
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

            Console.WriteLine(validationResult);

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
