namespace Data.Interfaces;

public interface IUser : IDbModel<int>
{
    string Email { get; set; }
    string FirstName { get; set; }
    string LastName { get; set; }
    string Username { get; set; }
    string PhoneNumber { get; set; }
}
