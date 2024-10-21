namespace MyUniversity.Models.Request
{
    public class CreateStudentRequest
    {
        public string StudentID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public int MajorID { get; set; }
    }
}
