using System.Globalization;

public interface IYearExtractor
{
    public List<string> GetYearStrings(List<WeatherRecord> records);

}

public class YearExtractor : IYearExtractor
{
    public List<string> GetYearStrings(List<WeatherRecord> records)
    {
        List<string> yearStrings = new();

        foreach (var record in records)
        {
            yearStrings.Add(record.DateOfRecord.Year.ToString());
        }

        var sortedYears = yearStrings.OrderBy(c => int.Parse(c)).ToList();

        return sortedYears.Distinct().ToList();
    }
}