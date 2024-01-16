using System.ComponentModel.DataAnnotations;

using Data.Interfaces;

namespace Data.Abstracts;

public class User : DbModel<int>, IUser
{
    [Key]
    public override int Id { get; set; }

    [MaxLength(32)]
    public string ClerkId { get; set; }

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
}
