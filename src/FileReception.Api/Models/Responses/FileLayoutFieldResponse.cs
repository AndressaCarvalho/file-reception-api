namespace FileReception.Api.Models.Responses;

public class FileLayoutFieldResponse
{
    public int Id { get; set; }
    public int FileLayoutId { get; set; }
    public string FileLayoutName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Start { get; set; }
    public int End { get; set; }
    public int FileLayoutFieldTypeId { get; set; }
    public string FileLayoutFieldTypeName { get; set; } = string.Empty;
}
