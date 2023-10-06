using System.ComponentModel.DataAnnotations;

namespace Data.Models;

public class Photo
{
    [Key]
    public int PhotoId { get; set; }

    public byte[] Data { get; set; }
}
