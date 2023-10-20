using System.ComponentModel.DataAnnotations.Schema;
using Data.Abstracts;

namespace Data.Models;

public class Employee : User
{
    public string PhoneNumber { get; set; }

    [ForeignKey(nameof(Department))]
    public int DepartmentId { get; set; }
    public Department Department { get; set; }
}
