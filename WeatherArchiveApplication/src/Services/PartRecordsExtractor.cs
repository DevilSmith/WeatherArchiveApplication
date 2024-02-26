using WeatherArchiveApp.Models;

public interface IPartRecordsExtractor
{
    public List<WeatherRecord> GetPartOfRecords(string month, string year, int countOfEntries);
}

public class PartRecordsExtractor : IPartRecordsExtractor
{
    public List<WeatherRecord> GetPartOfRecords(string month, string year, int countOfEntries)
    {
        List<WeatherRecord> partOfRecords = new();

        using (ApplicationContext db = new ApplicationContext())
        {
            partOfRecords = db.WeatherRecords
              .Where(r => r.DateOfRecord.Year.ToString() == year)
              .Where(r => r.DateOfRecord.ToString("MMMM") == month)
              .ToList();

            if (partOfRecords.Count() == 0)
            {

            }
        }

        return partOfRecords;

    }
}
