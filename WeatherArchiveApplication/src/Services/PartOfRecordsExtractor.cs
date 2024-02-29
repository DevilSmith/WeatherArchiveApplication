using WeatherArchiveApp.Models;

public interface IPartOfRecordsExtractor
{
    public List<WeatherRecord> GetPartOfRecords(string year, string month, List<WeatherRecord> allRecords);
}

public class PartOfRecordsExtractor : IPartOfRecordsExtractor
{
    List<WeatherRecord> partOfRecords = new();

    public List<WeatherRecord> GetPartOfRecords(string? year, string? month, List<WeatherRecord> allRecords)
    {
        partOfRecords = allRecords
          .Where(r => r.DateOfRecord.Year.ToString() == year)
          .Where(r => r.DateOfRecord.ToString("MMMM") == month)
          .OrderBy(r => r.TimeOfRecord)
          .OrderBy(r => r.DateOfRecord)
          .ToList();

        return partOfRecords;
    }
}
