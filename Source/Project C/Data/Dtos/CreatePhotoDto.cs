using System.ComponentModel.DataAnnotations;

using Data.Models;

namespace Data.Dtos;

public class CreatePhotoDto
{
    public int TicketId { get; set; }

    [MaxLength(40)]
    public string CreatedBy { get; set; }

    [MaxLength(256)]
    public string Name { get; set; }

    [MaxLength(20 * 1024 * 1024)]
    public string Data { get; set; }

    public Photo ToModel() => new()
    {
        CreatedBy = CreatedBy,
        UpdatedBy = CreatedBy,
        TicketId = TicketId,
        Name = Name,
        Data = Data
    };
}
