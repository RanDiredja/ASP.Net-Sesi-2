using System;
using System.ComponentModel.DataAnnotations;

namespace MyUniversity.Models.Request;

public class CreateStudentRequest
{
    // [Required]
    // public string StudentId { get; set; }

    [Required]
    [StringLength(50)]
    public string FirstName { get; set; }

    [Required]
    [StringLength(50)]
    public string LastName { get; set; }

    [Required]
    public int Age { get; set; }

    [Required]
    public int MajorId { get; set; }
}
