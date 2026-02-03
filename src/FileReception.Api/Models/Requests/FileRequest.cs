namespace FileReception.Api.Models.Requests;

public class FileRequest
{
    public int FileLayoutId { get; set; }
    public string FileName { get; set; } = string.Empty;
    public DateTime? ExpectedDate { get; set; }
    public int? StatusId { get; set; }
}
