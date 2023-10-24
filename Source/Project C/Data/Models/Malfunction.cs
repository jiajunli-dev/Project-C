using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Data.Abstracts;
using Data.Enums;

namespace Data.Models;

public class Malfunction : DbModel<int>
{
    public Priority Priority { get; set; }
 
    public Status Status { get; set; }
    
    [MaxLength(2048)]
    public string Description { get; set; }
    
    [MaxLength(2048)]
    public string Solution { get; set; }

    // Navigation properties
    [ForeignKey(nameof(Ticket))]
    public int TicketId { get; set; }
    
    public Ticket Ticket { get; set; }
}
