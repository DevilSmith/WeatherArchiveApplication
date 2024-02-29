public interface IFileExtensionValidator<T, M>
{
    public M ValidateFileExtension(T file);
}

public class ExcelFileExtensionValidator : IFileExtensionValidator<IFormFile, bool>
{
    public bool ValidateFileExtension(IFormFile file)
    {
        if (System.IO.Path.GetExtension(file.FileName) == ".xlsx") return true;
        else return false;
    }
}

public interface IFileDataValidator<T>
{
    public bool ValidateFileData(T file);
}

public class ExcelFileModelValidator : IFileDataValidator<IFormFile>
{
    private readonly IDataExtractor<List<WeatherRecord>, IFormFile> _dataExtractor;

    public ExcelFileModelValidator(IDataExtractor<List<WeatherRecord>, IFormFile> dataExtractor)
    {
        _dataExtractor = dataExtractor;
    }

    public bool ValidateFileData(IFormFile file)
    {
        List<WeatherRecord> extractedData = new();

        try
        {
            extractedData = _dataExtractor.ExtractData(file);
        }
        catch
        {
            return false;
        }

        return true;
    }
}
