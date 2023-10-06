using System.ComponentModel.DataAnnotations;

namespace Data.Models
{
    public class Malfunction
    {
        [Key]
        public int MalfunctionId { get; set; }
        public Priority priority { get; set; }

        [MaxLength(2048)]
        public string Description { get; set; }
        public int TicketId { get; set; }

        [MaxLength(2048)]
        public string Solution { get; set; }
    }
}
