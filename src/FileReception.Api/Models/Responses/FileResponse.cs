namespace FileReception.Api.Models.Responses;

public class FileResponse
{
    public int Id { get; set; }
    public int FileLayoutId { get; set; }
    public string FileLayoutName { get; set; } = string.Empty;
    public int CompanyId { get; set; }
    public string CompanyName { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;
    public DateTime ExpectedDate { get; set; }
    public int StatusId { get; set; }
    public string StatusName { get; set; } = string.Empty;
}
