using Data.Models;

namespace Data.Dtos;

public class CreatePhotoDto
{
    public string CreatedBy { get; set; }
    public int TicketId { get; set; }

    public string Name { get; set; }
    public string Data { get; set; }

    public void SetData(byte[] data) => Data = Convert.ToBase64String(data);

    public Photo ToPhoto() => new()
    {
        CreatedBy = CreatedBy,
        UpdatedBy = CreatedBy,
        TicketId = TicketId,
        Name = Name,
        Data = Convert.FromBase64String(Data),
    };
}