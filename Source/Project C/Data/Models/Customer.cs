using System.ComponentModel.DataAnnotations;

using Data.Abstracts;

namespace Data.Models;

public class Customer : User
{
    [MaxLength(64)]
    public string CompanyName { get; set; }

    [MaxLength(16)]
    public string CompanyPhoneNumber { get; set; }

    [MaxLength(64)]
    public string DepartmentName { get; set; }
}
