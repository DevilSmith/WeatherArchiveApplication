using System.Globalization;

public interface IMonthExtractor
{
    public List<string> GetMonthStrings(List<WeatherRecord> records);

}
public class MonthExtractor : IMonthExtractor
{
    public List<string> GetMonthStrings(List<WeatherRecord> records)
    {
        List<string> monthStrings = new();

        foreach (var record in records)
        {
            monthStrings.Add(record.DateOfRecord.ToString("MMMM"));
        }

        var sortedMonths = monthStrings
            .Select(x => new { Name = x, Sort = DateOnly.ParseExact(x, "MMMM", CultureInfo.InvariantCulture) })
            .OrderBy(x => x.Sort)
            .Select(x => x.Name);

        return sortedMonths.Distinct().ToList();
    }
}

