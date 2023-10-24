namespace Data.Interfaces;

public interface IDbModel<T> 
{
    T Id { get; set; }
}