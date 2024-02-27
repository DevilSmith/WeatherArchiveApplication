using System.Globalization;

public interface IMonthExtractor<T>
{
    public List<T> GetMonths(List<WeatherRecord> records, string year);
    public List<T> GetMonthsForEarlyYear(List<WeatherRecord> records);
}

public interface IMonthSorter
{
    public List<string> SortMonthsStrings(List<WeatherRecord> records);

}

public class MonthStringExtractor : IMonthExtractor<string>
{
    List<string> monthStrings = new();

    public List<string> GetMonthsForEarlyYear(List<WeatherRecord> records)
    {
        var earlistDate = records
                              .OrderBy(r => r.DateOfRecord).First();

        foreach (var record in records)
        {
            if (record.DateOfRecord.Year == earlistDate.DateOfRecord.Year) monthStrings.Add(record.DateOfRecord.ToString("MMMM"));
        }

        var sortedMonths = monthStrings
            .Select(x => new { Name = x, Sort = DateOnly.ParseExact(x, "MMMM", CultureInfo.InvariantCulture) })
            .OrderBy(x => x.Sort)
            .Select(x => x.Name);

        return monthStrings;

    }

    public List<string> GetMonths(List<WeatherRecord> records, string year)
    {
        foreach (var record in records)
        {
            if (record.DateOfRecord.Year.ToString() == year) monthStrings.Add(record.DateOfRecord.ToString("MMMM"));
        }

        var sortedMonths = monthStrings
            .Select(x => new { Name = x, Sort = DateOnly.ParseExact(x, "MMMM", CultureInfo.InvariantCulture) })
            .OrderBy(x => x.Sort)
            .Select(x => x.Name);

        return sortedMonths.Distinct().ToList();
    }
}



