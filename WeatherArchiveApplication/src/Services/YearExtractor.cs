public interface IYearExtractor<T>
{
    public List<string> GetYears(List<WeatherRecord> records);

}

public class YearStringExtractor : IYearExtractor<string>
{
    public List<string> GetYears(List<WeatherRecord> records)
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
