using System.ComponentModel.DataAnnotations;

using Data.Interfaces;

namespace Data.Abstracts;

public class User : DbModel<string>, IUser
{
    [MaxLength(32)]
    public string Username { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
}
