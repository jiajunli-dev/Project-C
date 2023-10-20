using Data.Abstracts;

namespace Data.Models
{
    public class Department : DbModel<int>
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}