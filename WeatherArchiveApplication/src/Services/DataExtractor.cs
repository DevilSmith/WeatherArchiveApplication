using System.Globalization;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

public interface IDataExtractor<T, M>
{
    public T ExtractData(M data);
}

public class ExcelDataExtractor : IDataExtractor<List<WeatherRecord>, IFormFile>
{
    public List<WeatherRecord> ExtractData(IFormFile data)
    {
        List<WeatherRecord> extractedWeatherRecords = new();

        IWorkbook workbook;
        using (MemoryStream memStream = new())
        {
            data.CopyTo(memStream);
            memStream.Position = 0;
            workbook = new XSSFWorkbook(memStream);
        }

        for (int i = 0; i < workbook.NumberOfSheets; i++)
        {
            ISheet sheet = workbook.GetSheetAt(i);

            if (sheet.PhysicalNumberOfRows == 0) throw new ExcelDataExtractionException();

            for (int j = 4; j < sheet.PhysicalNumberOfRows; j++)
            {
                IRow row = sheet.GetRow(j);

                WeatherRecord record = new WeatherRecord();

                try
                {
                    record.DateOfRecord = DateOnly.ParseExact(row.GetCell(0).StringCellValue, "dd.MM.yyyy");
                }
                catch
                {
                    throw new ExcelDataExtractionException();
                }

                try
                {
                    record.TimeOfRecord = TimeOnly.ParseExact(row.GetCell(1).StringCellValue, "HH:mm", CultureInfo.InvariantCulture);
                }
                catch
                {
                    throw new ExcelDataExtractionException();
                }

                try
                {
                    record.Temperature = (float)row.GetCell(2).NumericCellValue;
                }
                catch
                {
                    throw new ExcelDataExtractionException();
                }

                try
                {
                    record.Humidity = (float)row.GetCell(3).NumericCellValue;
                }
                catch
                {
                    throw new ExcelDataExtractionException();
                }

                try
                {
                    record.Dewpoint = (float)row.GetCell(4).NumericCellValue;
                }
                catch
                {
                    throw new ExcelDataExtractionException();
                }

                try
                {
                    record.AtmosPressure = (ushort)row.GetCell(5).NumericCellValue;
                }
                catch
                {
                    throw new ExcelDataExtractionException();
                }

                try
                {
                    record.WindDirection = row.GetCell(6).StringCellValue;
                }
                catch
                {
                    throw new ExcelDataExtractionException();
                }

                try
                {
                    record.WindSpeed = (byte)row.GetCell(7).NumericCellValue;
                }
                catch
                {
                    record.WindSpeed = row.GetCell(7).StringCellValue == " " ? null : throw new ExcelDataExtractionException();
                }

                try
                {
                    record.Overcast = (byte)row.GetCell(8).NumericCellValue;
                }
                catch
                {
                    record.Overcast = row.GetCell(8).StringCellValue == " " ? null : throw new ExcelDataExtractionException();
                }

                try
                {
                    record.OvercastLowerLimit = (ushort)row.GetCell(9).NumericCellValue;
                }
                catch
                {
                    record.OvercastLowerLimit = row.GetCell(9).StringCellValue == " " ? null : throw new ExcelDataExtractionException();
                }

                try
                {
                    record.HorizontalVisibility = row.GetCell(10).NumericCellValue.ToString();
                }
                catch
                {
                    record.HorizontalVisibility = row.GetCell(10).StringCellValue;
                }

                record.WeatherEvents = row?.GetCell(11)?.StringCellValue;

                extractedWeatherRecords.Add(record);

            }
        }

        return extractedWeatherRecords;

    }
}

public class ExcelDataExtractionException : Exception
{
    public ExcelDataExtractionException()
    {
    }
}
