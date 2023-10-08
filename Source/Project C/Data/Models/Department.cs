using System.ComponentModel.DataAnnotations;

namespace Data.Models
{
    public class Department
    {
        [Key]
        public int DepartmentId { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
    }
}