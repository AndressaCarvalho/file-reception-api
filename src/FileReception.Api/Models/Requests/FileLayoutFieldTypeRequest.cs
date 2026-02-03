using System.ComponentModel.DataAnnotations;

namespace FileReception.Api.Models.Requests;

public class FileLayoutFieldTypeRequest
{
    [Required]
    public string Name { get; set; } = string.Empty;
}
