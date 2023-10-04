using System.ComponentModel.DataAnnotations;

namespace Data.Models;

public class Ticket
{
  [Key]
  public int TicketId { get; set; }
  public int UserId { get; set; }

  [MaxLength(2048)]
  public string Description { get; set; }
  [MaxLength(2048)]
  public string TriedSolutions { get; set; }
  [MaxLength(2048)]
  public string AdditionalNotes { get; set; }

  public Priority Priority { get; set; }
}
