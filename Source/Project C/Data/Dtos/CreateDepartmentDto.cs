using System.ComponentModel.DataAnnotations;

using Data.Enums;
using Data.Models;

namespace Data.Dtos;

public class CreateDepartmentDto
{
    public string CreatedBy { get; set; }

    public string Name { get; set; }
    public string Description { get; set; }

    public Department ToModel() => new()
    {
        CreatedBy = CreatedBy,
        UpdatedBy = CreatedBy,
        Name = Name,
        Description = Description,

    };
}
