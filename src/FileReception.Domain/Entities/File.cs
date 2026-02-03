namespace FileReception.Domain.Entities;

public class File : BaseEntity
{
    public int FileLayoutId { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime ExpectedDate { get; set; }
    public int StatusId { get; set; }

    public virtual FileLayout FileLayout { get; set; }
    public virtual FileStatus FileStatus { get; set; }
    public ICollection<FileProcess> Processes { get; set; } = new List<FileProcess>();
}
