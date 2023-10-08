using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Models;

public class Employee
{
    // TODO: Check what Id Clerk provides, and adjust accordingly
    [Key]
    public int UserId { get; set; }

    public string PhoneNumber { get; set; }

    // TODO: Implement after implementation of Department
    //// Navigation Properties
    //[ForeignKey(nameof(Department))]
    //public int DepartmentId { get; set; }
    //public Department Department { get; set; }
}
