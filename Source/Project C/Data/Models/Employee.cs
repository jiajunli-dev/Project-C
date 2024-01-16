using System.ComponentModel.DataAnnotations.Schema;

using Data.Abstracts;

namespace Data.Models;

public class Employee : User
{
    [ForeignKey(nameof(Department))]
    public int DepartmentId { get; set; }
    
    public Department Department { get; set; }

    public string Role { get; set; }
}
