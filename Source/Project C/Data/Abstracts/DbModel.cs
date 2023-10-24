using System.ComponentModel.DataAnnotations;

using Data.Interfaces;

namespace Data.Abstracts;

public abstract class DbModel<T> : IDbModel<T>, ICreatable
{
    [Key]
    public virtual T Id { get; set; }

    [DataType(DataType.DateTime)]
    public DateTime CreatedAt { get; set; }

    [MaxLength(40)]
    public string CreatedBy { get; set; }

    [DataType(DataType.DateTime)]
    public DateTime UpdatedAt { get; set; }

    [MaxLength(40)]
    public string UpdatedBy { get; set; }
}