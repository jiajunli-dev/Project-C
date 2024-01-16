using System.ComponentModel.DataAnnotations;

using Data.Models;

namespace Data.Dtos;

public class EmployeeDto
{
    public EmployeeDto() { }
    public EmployeeDto(Employee employee)
    {
        CreatedAt = employee.CreatedAt;
        CreatedBy = employee.CreatedBy;
        UpdatedAt = employee.UpdatedAt;
        UpdatedBy = employee.UpdatedBy;
        Username = employee.Username;
        FirstName = employee.FirstName;
        LastName = employee.LastName;
        Email = employee.Email;
        PhoneNumber = employee.PhoneNumber;
        DepartmentId = employee.DepartmentId;
    }

    [MaxLength(40)]
    public int Id { get; set; }

    [DataType(DataType.DateTime)]
    public DateTime CreatedAt { get; set; }

    [MaxLength(40)]
    public string CreatedBy { get; set; }

    [DataType(DataType.DateTime)]
    public DateTime UpdatedAt { get; set; }

    [MaxLength(40)]
    public string UpdatedBy { get; set; }

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