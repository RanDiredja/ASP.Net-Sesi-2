using System.ComponentModel.DataAnnotations;

namespace MyUniversity.Models.Request
{
    public class CreateStudentRequest
    {
        [Required]
        public string StudentID { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public int Age { get; set; }
        [Required]
        public int MajorID { get; set; }
    }
}
