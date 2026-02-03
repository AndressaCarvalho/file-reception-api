namespace FileReception.Domain.Entities;

public class FileLayoutField : BaseEntity
{
    public int FileLayoutId { get; set; }
    public string Description { get; set; } = string.Empty;
    public int Start { get; set; }
    public int End { get; set; }
    public int FileLayoutFieldTypeId { get; set; }

    public virtual FileLayout FileLayout { get; set; }
    public virtual FileLayoutFieldType FileLayoutFieldType { get; set; }
}
