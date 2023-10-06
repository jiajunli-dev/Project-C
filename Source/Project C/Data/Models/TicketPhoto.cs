using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Models;

public class TicketPhoto
{
    [Key]
    public int TicketId { get; set; }
    [ForeignKey(nameof(Ticket))]
    public int PhotoId { get; set; }

    // Navigation properties
    public Photo Photo { get; set; }
    public Ticket Ticket { get; set; }
}
