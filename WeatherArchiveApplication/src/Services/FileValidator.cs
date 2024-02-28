using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.Util;
using NPOI.HSSF.Util;

public interface IFileExtensionValidator<T, M>
{
    public M ValidateFileExtension(T file);
}

public class ExcelFileExtensionValidator : IFileExtensionValidator<IFormFile, bool>
{
    public bool ValidateFileExtension(IFormFile file)
    {
        if (System.IO.Path.GetExtension(file.FileName) == ".xlsx") return true;
        else return false;
    }
}

public interface IFileDataValidator<T>
{
    public bool ValidateFileData(T file);
}

public class ExcelFileModelValidator : IFileDataValidator<IFormFile>
{
    public bool ValidateFileData(IFormFile file)
    {
        IWorkbook workbook;
        using (MemoryStream memStream = new())
        {
            file.CopyTo(memStream);
            memStream.Position = 0;
            workbook = new XSSFWorkbook(memStream);
        }

        // Получение листа
        ISheet sheet = workbook.GetSheetAt(0);

        // Чтение данных из ячейки
        IRow row = sheet.GetRow(0);
        string cellValue = row.GetCell(0).StringCellValue;

        Console.WriteLine("Getted cell: ");
        Console.WriteLine(cellValue);

        return false;
    }
}
