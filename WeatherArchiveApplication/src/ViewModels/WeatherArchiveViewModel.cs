namespace WeatherArchiveApp.ViewModels
{
    public class WeatherArchiveViewModel
    {
        public List<WeatherRecord> WeatherRecords { get; set; } = new();
        public List<string> MonthStrings { get; set; } = new();
        public List<string> YearStrings { get; set; } = new();
    }
}
