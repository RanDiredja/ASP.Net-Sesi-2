using System;

namespace MyUniversity.Models.Request;

public class UpdateStudentRequest
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int Age { get; set; }
    public int MajorID { get; set; }
}
