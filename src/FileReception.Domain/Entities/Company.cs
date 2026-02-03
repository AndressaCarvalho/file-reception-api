namespace FileReception.Domain.Entities;

public class Company : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public ICollection<FileLayout> FileLayouts { get; set; } = new List<FileLayout>();
}
