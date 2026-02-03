using System.ComponentModel.DataAnnotations;

namespace FileReception.Api.Models.Requests;

public class FileLayoutRequest
{
    [Required]
    public string Name { get; set; } = string.Empty;
    [Required]
    public int CompanyId { get; set; }
}
