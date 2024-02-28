namespace WeatherArchiveApp.ViewModels
{
    public class TableOfRecordsViewModel
    {
        public List<WeatherRecord> WeatherRecords { get; set; } = new();
        public List<string> MonthStrings { get; set; } = new();
        public List<string> YearStrings { get; set; } = new();
        public string SelectedYear { get; set; } = "";
        public string SelectedMonth { get; set; } = "";
    }
}
