using System.ComponentModel.DataAnnotations;

using Data.Models;

namespace Data.Dtos;

public class EmployeeDto
{
    public EmployeeDto() { }
    public EmployeeDto(Employee malfunction)
    {
        Id = malfunction.Id;
        CreatedAt = malfunction.CreatedAt;
        CreatedBy = malfunction.CreatedBy;
        UpdatedAt = malfunction.UpdatedAt;
        UpdatedBy = malfunction.UpdatedBy;
        Username = malfunction.Username;
        FirstName = malfunction.FirstName;
        LastName = malfunction.LastName;
        Email = malfunction.Email;
        PhoneNumber = malfunction.PhoneNumber;
        DepartmentId = malfunction.DepartmentId;
    }

    public string Id { get; set; }
    [DataType(DataType.DateTime)]
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; }

    [DataType(DataType.DateTime)]
    public DateTime UpdatedAt { get; set; }
    public string UpdatedBy { get; set; }

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
        CreatedAt = CreatedAt,
        CreatedBy = CreatedBy,
        UpdatedAt = UpdatedAt,
        UpdatedBy = UpdatedBy,
        Username = Username,
        FirstName = FirstName,
        LastName = LastName,
        Email = Email,
        PhoneNumber = PhoneNumber,
        DepartmentId = DepartmentId
    };
}