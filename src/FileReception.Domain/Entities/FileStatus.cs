namespace FileReception.Domain.Entities;

public class FileStatus : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public ICollection<File> Files { get; set; } = new List<File>();
}
