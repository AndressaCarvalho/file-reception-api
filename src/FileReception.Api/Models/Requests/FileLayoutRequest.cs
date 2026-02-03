namespace FileReception.Api.Models.Requests;

public class FileLayoutRequest
{
    public string Name { get; set; } = string.Empty;
    public int CompanyId { get; set; }
}
