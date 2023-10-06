using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Models;

public class Employee
{
    [Key]
    public int DepartmentId { get; set; }

    [ForeignKey("UserId")]
    public int UserId { get; set; }
    public string PhoneNumber { get; set; }
}