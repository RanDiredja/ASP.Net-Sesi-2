using System.ComponentModel.DataAnnotations;

namespace MyUniversity.Models
{
    public class Major
    {
        [Key]
        public int MajorID { get; set; }

        [MaxLength(50)]
        public string MajorName { get; set; }

        public ICollection<Student> Students { get; set; }
    }
}
