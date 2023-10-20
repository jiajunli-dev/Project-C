using Data.Models;

namespace Data.Dtos;

public class CreatePhotoDto
{
    public int TicketId { get; set; }
    public string CreatedBy { get; set; }

    public string Name { get; set; }
    public string Data { get; set; }

    public Photo ToModel() => new()
    {
        CreatedBy = CreatedBy,
        UpdatedBy = CreatedBy,
        TicketId = TicketId,
        Name = Name,
        Data = Convert.FromBase64String(Data),
    };
}
