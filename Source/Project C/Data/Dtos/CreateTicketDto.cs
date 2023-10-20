using System.ComponentModel.DataAnnotations;

using Data.Enums;
using Data.Models;

namespace Data.Dtos;

public class CreateTicketDto
{
    public string CreatedBy { get; set; }
    [MaxLength(2048)]
    public string Description { get; set; }
    [MaxLength(2048)]
    public string TriedSolutions { get; set; }
    [MaxLength(2048)]
    public string AdditionalNotes { get; set; }

    public Priority Priority { get; set; }


    public Ticket ToTicket() => new()
    {
        CreatedBy = CreatedBy,
        UpdatedBy = CreatedBy,
        Description = Description,
        TriedSolutions = TriedSolutions,
        AdditionalNotes = AdditionalNotes,
        Priority = Priority,
    };
}
