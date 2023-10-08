using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Models
{
    public class Malfunction
    {
        [Key]
        public int MalfunctionId { get; set; }

        public Priority Priority { get; set; }
        [MaxLength(2048)]
        public string Description { get; set; }
        [MaxLength(2048)]
        public string Solution { get; set; }

        // Navigation properties
        [ForeignKey(nameof(Ticket))]
        public int TicketId { get; set; }
        public Ticket Ticket { get; set; }
    }
}
