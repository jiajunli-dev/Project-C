using System.ComponentModel.DataAnnotations;
namespace Data.Models;

public class User
{
    [Key]
    public int UserId { get; set; }

    [MaxLength(32)]
    public string Username { get; set; }
    public int RoleId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }

    public Ticket CreateTicket()
    {
        throw new NotImplementedException();
    }
}
