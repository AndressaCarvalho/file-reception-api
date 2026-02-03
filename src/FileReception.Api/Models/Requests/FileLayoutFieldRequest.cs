namespace FileReception.Api.Models.Requests;

public class FileLayoutFieldRequest
{
    public int FileLayoutId { get; set; }
    public string Description { get; set; } = string.Empty;
    public int Start { get; set; }
    public int End { get; set; }
    public int FileLayoutFieldTypeId { get; set; }
}
