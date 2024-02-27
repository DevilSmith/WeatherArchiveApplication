using System.Globalization;

public interface IMonthExtractor<T>
{
    public List<T> GetMonths(List<WeatherRecord> records, string year);
}

public interface IMonthSorter<T>
{
    public List<T> SortMonthsStrings(List<T> monthStrings);

}

public class MonthStringSorter : IMonthSorter<string>
{
    public List<string> SortMonthsStrings(List<string> monthStrings)
    {
        var sortedMonths = monthStrings
            .Select(x => new { Name = x, Sort = DateOnly.ParseExact(x, "MMMM", CultureInfo.InvariantCulture) })
            .OrderBy(x => x.Sort)
            .Select(x => x.Name)
            .Distinct()
            .ToList();

        return sortedMonths;
    }
}

public class MonthStringExtractor : IMonthExtractor<string>
{
    private readonly IMonthSorter<string> _monthSorter;
    List<string> monthStrings = new();

    public MonthStringExtractor(IMonthSorter<string> monthSorter)
    {
        _monthSorter = monthSorter;
    }

    public List<string> GetMonths(List<WeatherRecord> records, string year)
    {
        foreach (var record in records)
        {
            if (record.DateOfRecord.Year.ToString() == year) monthStrings.Add(record.DateOfRecord.ToString("MMMM"));
        }

        var sortedMonths = _monthSorter.SortMonthsStrings(monthStrings);

        return sortedMonths;
    }
}



