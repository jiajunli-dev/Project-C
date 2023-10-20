namespace Data.Interfaces;

public interface IUser : IDbModel<string>
{
    string Email { get; set; }
    string FirstName { get; set; }
    string LastName { get; set; }
    string Username { get; set; }
}