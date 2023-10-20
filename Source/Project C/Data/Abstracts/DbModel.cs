using System.ComponentModel.DataAnnotations;

using Data.Interfaces;

namespace Data.Abstracts;

public abstract class DbModel<T> : IDbModel<T>
{
    [Key]
    public T Id { get; set; }

    [DataType(DataType.DateTime)]
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; }

    [DataType(DataType.DateTime)]
    public DateTime UpdatedAt { get; set; }
    public string UpdatedBy { get; set; }
}