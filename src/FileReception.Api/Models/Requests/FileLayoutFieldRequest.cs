using System.ComponentModel.DataAnnotations;

namespace FileReception.Api.Models.Requests;

public class FileLayoutFieldRequest
{
    [Required]
    public int FileLayoutId { get; set; }
    [Required]
    public string Description { get; set; } = string.Empty;
    [Required]
    public int Start { get; set; }
    [Required]
    public int End { get; set; }
    [Required]
    public int FileLayoutFieldTypeId { get; set; }
}
