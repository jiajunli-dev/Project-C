using System.ComponentModel.DataAnnotations;

using Data.Abstracts;
using Data.Enums;

namespace Data.Models;

public class Ticket : DbModel<int>
{
    [MaxLength(2048)]
    public string Description { get; set; }
    [MaxLength(2048)]
    public string TriedSolutions { get; set; }
    [MaxLength(2048)]
    public string AdditionalNotes { get; set; }

    public Priority Priority { get; set; }

    public Status Status { get; set; }
}
