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

    public string DataToBase64()
        => Data is null ? string.Empty : Convert.ToBase64String(Data);

    public static Photo FromDto(PhotoDto dto) => new()
    {
        PhotoId = dto.PhotoId,
        Name = dto.Name,
        Data = Convert.FromBase64String(dto.Data),
        TicketId = dto.TicketId,
    };
}

public class PhotoDto
{
    public int PhotoId { get; set; }
    public string Name { get; set; }
    public string Data { get; set; } = string.Empty;
    public int TicketId { get; set; }

    public static PhotoDto FromPhoto(Photo photo) => new()
    {
        PhotoId = photo.PhotoId,
        Name = photo.Name,
        Data = photo.DataToBase64(),
        TicketId = photo.TicketId,
    };
}