using System.ComponentModel.DataAnnotations;

using Data.Enums;
using Data.Models;

namespace Data.Dtos;

public class MalfunctionDto
{
    public MalfunctionDto() { }
    public MalfunctionDto(Malfunction malfunction)
    {
        Id = malfunction.Id;
        CreatedAt = malfunction.CreatedAt;
        CreatedBy = malfunction.CreatedBy;
        UpdatedAt = malfunction.UpdatedAt;
        UpdatedBy = malfunction.UpdatedBy;
        Priority = malfunction.Priority;
        Status = malfunction.Status;
        Description = malfunction.Description;
        Solution = malfunction.Solution;
        TicketId = malfunction.TicketId;
    }

    public int Id { get; set; }
    [DataType(DataType.DateTime)]
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; }

    [DataType(DataType.DateTime)]
    public DateTime UpdatedAt { get; set; }
    public string UpdatedBy { get; set; }

    public Priority Priority { get; set; }
    public Status Status { get; set; }
    [MaxLength(2048)]
    public string Description { get; set; }
    [MaxLength(2048)]
    public string Solution { get; set; }

    public int TicketId { get; set; }

    public Malfunction ToModel() => new()
    {
        Id = Id,
        CreatedAt = CreatedAt,
        CreatedBy = CreatedBy,
        UpdatedAt = UpdatedAt,
        UpdatedBy = UpdatedBy,
        Priority = Priority,
        Status = Status,
        Description = Description,
        Solution = Solution,
        TicketId = TicketId
    };
}
