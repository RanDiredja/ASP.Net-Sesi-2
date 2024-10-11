using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyUniversity.Models
{
    public class Student
    {
        [Key]
        [StringLength(5)]
        [RegularExpression("ST[0-9][0-9][0-9]$")]
        public string StudentID { get; set; }

        [MaxLength(50)]
        public string FirstName { get; set; }

        [MaxLength(50)]
        public string LastName { get; set; }

        public int Age { get; set; }

        [ForeignKey("Major")]
        public int MajorID { get; set; }

        public Major Major { get; set; }
    }
}
