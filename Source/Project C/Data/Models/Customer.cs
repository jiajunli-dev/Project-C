using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Models;

public class Customer
{
    [Key]
    public int UserId { get; set; }

    public string PhoneNumber { get; set; }
    public string CompanyName { get; set; }
    public string CompanyPhoneNumber { get; set; }
    public string DepartmentName { get; set; }
}