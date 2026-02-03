namespace FileReception.Domain.Entities;

public class FileLayout : BaseEntity
{
    public int CompanyId { get; set; }
    public string Name { get; set; } = string.Empty;

    public virtual Company Company { get; set; }
    public ICollection<File> Files { get; set; } = new List<File>();
    public virtual ICollection<FileLayoutField> Fields { get; set; } = new List<FileLayoutField>();
}
