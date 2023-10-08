using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Models;

public class Photo
{
    [Key]
    public int PhotoId { get; set; }

    public string Name { get; set; }
    public byte[] Data { get; set; }

    // Navigation properties
    [ForeignKey(nameof(Ticket))]
    public int TicketId { get; set; }
    public Ticket Ticket { get; set; }
}
