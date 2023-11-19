using System.ComponentModel.DataAnnotations;

using Data.Models;

namespace Data.Dtos;

public class CreateDepartmentDto
{
    [MaxLength(40)]
    public string CreatedBy { get; set; }

    [MaxLength(64)]
    public string Name { get; set; }

    [MaxLength(2048)]
    public string Description { get; set; }

    public Department ToModel() => new()
    {
        CreatedBy = CreatedBy,
        UpdatedBy = CreatedBy,
        Name = Name,
        Description = Description,

    };
}
