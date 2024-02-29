using System.ComponentModel.DataAnnotations;

namespace WeatherArchiveApp.ViewModels;

public class ReponseViewModel
{
    public string Message { get; set; } = "";
    public bool IsSuccess { get; set; }
    public bool IsResponse { get; set; }
}

public class UploaderFilesViewModel : ReponseViewModel
{
    [Required(ErrorMessage = "Please select files")]
    public List<IFormFile> Files { get; set; } = new();
}


