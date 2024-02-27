public enum DateParamsValidationResult
{
    ValidYear,
    ValidAll,
    InvalidAll
}

public interface IDateParamsValidator<T>
{
    public DateParamsValidationResult ValidateDateParams(T year, T month, List<WeatherRecord> records);
}

public class DateStringParamsValidator : IDateParamsValidator<string>
{
    public DateParamsValidationResult ValidateDateParams(string year, string month, List<WeatherRecord> records)
    {
        DateParamsValidationResult validationResult = DateParamsValidationResult.InvalidAll;

        foreach (var record in records)
        {
            if ((record.DateOfRecord.Year.ToString() == year) && (record.DateOfRecord.ToString("MMMM") == month)) { validationResult = DateParamsValidationResult.ValidAll; }
            else if (record.DateOfRecord.Year.ToString() == year) { validationResult = DateParamsValidationResult.ValidYear; }
        }

        return validationResult;
    }
}
