namespace FileReception.Api.Models.Responses;

public class FileLayoutResponse
{
    public int Id { get; set; }
    public string FileLayoutName { get; set; } = string.Empty;
    public int CompanyId { get; set; }
    public string CompanyName { get; set; } = string.Empty;
}
