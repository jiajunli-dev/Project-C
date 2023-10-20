using System.ComponentModel.DataAnnotations.Schema;

using Data.Abstracts;
using Data.Dtos;

namespace Data.Models;

public class Photo : DbModel<int>
{
    public Photo() { }
    public Photo(PhotoDto dto)
    {
        Id = dto.Id;
        CreatedAt = dto.CreatedAt;
        CreatedBy = dto.CreatedBy;
        UpdatedAt = dto.UpdatedAt;
        UpdatedBy = dto.UpdatedBy;
        Name = dto.Name;
        Data = Convert.FromBase64String(dto.Data);
        TicketId = dto.TicketId;
    }

    public string Name { get; set; }
    public byte[] Data { get; set; }

    // Navigation properties
    [ForeignKey(nameof(Ticket))]
    public int TicketId { get; set; }
    public Ticket Ticket { get; set; }

    public string DataToBase64() => Data is null ? string.Empty : Convert.ToBase64String(Data);
}