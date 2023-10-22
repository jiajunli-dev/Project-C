using System.ComponentModel.DataAnnotations;

using Data.Models;

namespace Data.Dtos;

public class CreateEmployeeDto
{
    public string Id { get; set; }
    public string CreatedBy { get; set; }

    [MaxLength(32)]
    public string Username { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }

    public int DepartmentId { get; set; }

    public Employee ToModel() => new()
    {
        Id = Id,
        CreatedBy = CreatedBy,
        UpdatedBy = CreatedBy,
        Username = Username,
        FirstName = FirstName,
        LastName = LastName,
        Email = Email,
        PhoneNumber = PhoneNumber,
        DepartmentId = DepartmentId,
    };
}