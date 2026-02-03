namespace FileReception.Domain.Entities;

public class FileLayoutFieldType : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public virtual ICollection<FileLayoutField> Fields { get; set; } = new List<FileLayoutField>();
}
