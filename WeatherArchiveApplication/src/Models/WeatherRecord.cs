public class WeatherRecord
{
    public int Id { get; set; }
    public DateOnly DateOfRecord { get; set; }
    public TimeOnly TimeOfRecord { get; set; }
    public float Temperature { get; set; }
    public float Humidity { get; set; }
    public float Dewpoint { get; set; }
    public ushort AtmosPressure { get; set; }
    public string? WindDirection { get; set; }
    public byte? WindSpeed { get; set; }
    public byte? Overcast { get; set; }
    public ushort OvercastLowerLimit { get; set; }
    public byte? HorizontalVisibility { get; set; }
    public string? WeatherEvents { get; set; }
}
