using System.ComponentModel.DataAnnotations;
namespace Data.Models;

public class User
{
    // TODO: Check what Id Clerk provides, and adjust accordingly
    [Key]
    public int UserId { get; set; }

    [MaxLength(32)]
    public string Username { get; set; }
    // TODO: Check if clerk hosts roles
    public int RoleId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }

    // TODO: Implement CreateTicket()
    public Ticket CreateTicket()
    {
        throw new NotImplementedException();
    }
}
