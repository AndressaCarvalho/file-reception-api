using System.ComponentModel.DataAnnotations;

namespace FileReception.Api.Models.Requests;

public class FileStatusRequest
{
    [Required]
    public string Name { get; set; } = string.Empty;
}
