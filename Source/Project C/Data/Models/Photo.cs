using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Data.Abstracts;

namespace Data.Models;

public class Photo : DbModel<int>
{
    [MaxLength(256)]
    public string Name { get; set; }

    [MaxLength(20 * 1024 * 1024)]
    public byte[] Data { get; set; }

    // Navigation properties
    [ForeignKey(nameof(Ticket))]
    public int TicketId { get; set; }

    public Ticket Ticket { get; set; }
}