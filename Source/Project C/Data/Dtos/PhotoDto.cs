using System.ComponentModel.DataAnnotations;

using Data.Models;

namespace Data.Dtos;

public class PhotoDto
{
    public PhotoDto() { }
    public PhotoDto(Photo photo)
    {
        Id = photo.Id;
        CreatedAt = photo.CreatedAt;
        CreatedBy = photo.CreatedBy;
        UpdatedAt = photo.UpdatedAt;
        UpdatedBy = photo.UpdatedBy;
        Name = photo.Name;
        Data = Convert.ToBase64String(photo.Data);
        TicketId = photo.TicketId;
    }

    public int Id { get; set; }
    [DataType(DataType.DateTime)]
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; }

    [DataType(DataType.DateTime)]
    public DateTime UpdatedAt { get; set; }
    public string UpdatedBy { get; set; }
    public string Name { get; set; }
    public string Data { get; set; } = string.Empty;
    public int TicketId { get; set; }

    public Photo ToModel() => new()
    {
        Id = Id,
        CreatedAt = CreatedAt,
        CreatedBy = CreatedBy,
        UpdatedAt = UpdatedAt,
        UpdatedBy = UpdatedBy,
        Name = Name,
        Data = Convert.FromBase64String(Data),
        TicketId = TicketId,
    };
}
