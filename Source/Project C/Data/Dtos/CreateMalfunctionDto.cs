using System.ComponentModel.DataAnnotations;

using Data.Enums;
using Data.Models;

namespace Data.Dtos;

public class CreateMalfunctionDto
{
    public int TicketId { get; set; }
    public string CreatedBy { get; set; }

    public Priority Priority { get; set; }
    public Status Status { get; set; }
    [MaxLength(2048)]
    public string Description { get; set; }
    [MaxLength(2048)]
    public string Solution { get; set; }

    public Malfunction ToModel() => new()
    {
        TicketId = TicketId,
        CreatedBy = CreatedBy,
        UpdatedBy = CreatedBy,
        Priority = Priority,
        Status = Status,
        Description = Description,
        Solution = Solution,
    };
}
