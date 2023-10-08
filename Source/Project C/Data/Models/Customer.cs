using System.ComponentModel.DataAnnotations;

namespace Data.Models;

public class Customer
{
    // TODO: Check what Id Clerk provides, and adjust accordingly
    [Key]
    public int UserId { get; set; }

    public string PhoneNumber { get; set; }
    public string CompanyName { get; set; }
    public string CompanyPhoneNumber { get; set; }
    public string DepartmentName { get; set; }
}
