using System.ComponentModel.DataAnnotations;

using Data.Models;

namespace Data.Dtos;

public class CreateEmployeeDto
{
    [MaxLength(40)]
    public string Id { get; set; }

    [MaxLength(40)]
    public string CreatedBy { get; set; }

    [MaxLength(32)]
    public string Username { get; set; }

    [MaxLength(254)]
    public string FirstName { get; set; }

    [MaxLength(254)]
    public string LastName { get; set; }

    [MaxLength(254)]
    public string Email { get; set; }

    [MaxLength(16)]
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