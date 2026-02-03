using System.ComponentModel.DataAnnotations;

namespace FileReception.Api.Models.Requests;

public class FileRequest
{
    [Required]
    public int FileLayoutId { get; set; }
    [Required]
    public string FileName { get; set; } = string.Empty;
    public DateTime? ExpectedDate { get; set; }
    public int? StatusId { get; set; }
}
