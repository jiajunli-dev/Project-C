using System.ComponentModel.DataAnnotations;

using Data.Abstracts;

namespace Data.Models;
public class Machine : DbModel<int>
{
    [MaxLength(64)]
    public string Name { get; set; }

    [MaxLength(2048)]
    public string Description { get; set; }
}
