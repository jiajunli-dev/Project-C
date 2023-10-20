using System.ComponentModel.DataAnnotations.Schema;

using Data.Abstracts;

namespace Data.Models;

public class Photo : DbModel<int>
{
    public string Name { get; set; }
    public byte[] Data { get; set; }

    // Navigation properties
    [ForeignKey(nameof(Ticket))]
    public int TicketId { get; set; }
    public Ticket Ticket { get; set; }
}