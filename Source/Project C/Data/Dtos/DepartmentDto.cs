using System.ComponentModel.DataAnnotations;

using Data.Models;

namespace Data.Dtos;

public class DepartmentDto
{
    public DepartmentDto() { }
    public DepartmentDto(Department department)
    {
        Id = department.Id;
        CreatedAt = department.CreatedAt;
        CreatedBy = department.CreatedBy;
        UpdatedAt = department.UpdatedAt;
        UpdatedBy = department.UpdatedBy;
        Name = department.Name;
        Description = department.Description;
    }

    public int Id { get; set; }

    [DataType(DataType.DateTime)]
    public DateTime CreatedAt { get; set; }

    [MaxLength(40)]
    public string CreatedBy { get; set; }

    [DataType(DataType.DateTime)]
    public DateTime UpdatedAt { get; set; }

    [MaxLength(40)]
    public string UpdatedBy { get; set; }

    [MaxLength(64)]
    public string Name { get; set; }

    [MaxLength(2048)]
    public string Description { get; set; }

    public Department ToModel() => new()
    {
        Id = Id,
        CreatedAt = CreatedAt,
        CreatedBy = CreatedBy,
        UpdatedAt = UpdatedAt,
        UpdatedBy = UpdatedBy,
        Name = Name,
        Description = Description,
    };
}
