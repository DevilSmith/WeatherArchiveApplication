using WeatherArchiveApp.Models;

public interface IPartOfRecordsExtractor
{
    public List<WeatherRecord> GetPartOfRecords(string year, string month, List<WeatherRecord> allRecords);
    public List<WeatherRecord> GetEarlistMonthPartOfRecords(string year, List<WeatherRecord> allRecords);
    public List<WeatherRecord> GetEarlistPartOfRecords(List<WeatherRecord> allRecords);
}

public class PartOfRecordsExtractor : IPartOfRecordsExtractor
{
    List<WeatherRecord> partOfRecords = new();

    public List<WeatherRecord> GetEarlistPartOfRecords(List<WeatherRecord> allRecords)
    {
        var earlistDate = allRecords
                              .OrderBy(r => r.DateOfRecord).First();

        partOfRecords = allRecords
          .Where(r => r.DateOfRecord.Year == earlistDate.DateOfRecord.Year)
          .Where(r => r.DateOfRecord.Month == earlistDate.DateOfRecord.Month)
          .ToList();

        return partOfRecords;
    }

    public List<WeatherRecord> GetEarlistMonthPartOfRecords(string year, List<WeatherRecord> allRecords)
    {
        var earlistDate = allRecords
                              .Where(r => r.DateOfRecord.Year.ToString() == year)
                              .OrderBy(r => r.DateOfRecord).First();

        partOfRecords = allRecords
          .Where(r => r.DateOfRecord.Year == earlistDate.DateOfRecord.Year)
          .Where(r => r.DateOfRecord.Month == earlistDate.DateOfRecord.Month)
          .ToList();

        return partOfRecords;
    }

    public List<WeatherRecord> GetPartOfRecords(string? year, string? month, List<WeatherRecord> allRecords)
    {
        partOfRecords = allRecords
          .Where(r => r.DateOfRecord.Year.ToString() == year)
          .Where(r => r.DateOfRecord.ToString("MMMM") == month)
          .ToList();

        return partOfRecords;
    }
}
