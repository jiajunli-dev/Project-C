using Data.Abstracts;

namespace Data.Models;

public class Customer : User
{
    public string PhoneNumber { get; set; }
    public string CompanyName { get; set; }
    public string CompanyPhoneNumber { get; set; }
    public string DepartmentName { get; set; }
}
