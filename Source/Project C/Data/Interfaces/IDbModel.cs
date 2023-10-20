namespace Data.Interfaces;

public interface IDbModel<T> : ICreatable
{
    T Id { get; set; }
}