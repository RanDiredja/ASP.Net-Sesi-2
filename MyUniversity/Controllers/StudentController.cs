using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileSystemGlobbing.Internal.PathSegments;
using MyUniversity.Data;
using MyUniversity.Models;
using MyUniversity.Models.Request;
using MyUniversity.Models.Result;


// Create an API to GET a list of students
// Create an API to POST a new student
// Create an API to GET a specific student by ID
// Create an API to PUT a student Information
// Create an API to DELETE a student by ID


namespace MyUniversity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly AppDbContext _context;

        public StudentController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetStudentResult>>> Get()
        {
            var listStudent = await _context.Student
            .Include(x => x.Major)
            .Select(x => new GetStudentResult{
                StudentId = x.StudentID,
                Name = $"{x.FirstName} {x.LastName}",
                MajorName = x.Major.MajorName
            }).ToListAsync();

            var response = new ApiResponse<IEnumerable<GetStudentResult>>{
                StatusCode = StatusCodes.Status200OK,
                RequestMethod = HttpContext.Request.Method,
                Data = listStudent
            };

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateStudentRequest request)
        {
            try
            {
                // var isStudentExists = await _context.Student.Where(x => x.StudentID == request.StudentId).AnyAsync();
                // if(isStudentExists){
                //     throw new ArgumentException("Student is already exists");
                // }

                var isMajorExists = await _context.Major.Where(x => x.MajorID == request.MajorId).AnyAsync();
                if(!isMajorExists){
                    throw new KeyNotFoundException("Major data not found");
                }

                var topStudentId = await _context.Student
                .OrderByDescending(x => x.StudentID)
                .Select(x => x.StudentID)
                .FirstOrDefaultAsync();

                var substringId = topStudentId?.Substring(2);

                var currentId = int.Parse(substringId);

                currentId += 1;

                var studentIdNew = currentId.ToString("D3");

                var studentData = new Student{
                    StudentID = $"ST{studentIdNew}",
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Age = request.Age,
                    MajorID = request.MajorId
                };

                _context.Student.Add(studentData);
                await _context.SaveChangesAsync();

                var response = new ApiResponse<string>{
                    StatusCode = StatusCodes.Status201Created,
                    RequestMethod = HttpContext.Request.Method,
                    Data = "Student data saved successfully"
                };

                return Ok(response);
            }
            catch(Exception ex)
            {
                var response = new ApiResponse<string>{
                    StatusCode = StatusCodes.Status400BadRequest,
                    RequestMethod = HttpContext.Request.Method,
                    Data = ex.Message
                };
                return BadRequest(response);
            }
            
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetStudentResult>> Get(string id)
        {
            var studentData = await _context.Student
            .Include(x => x.Major)
            .Where(x => x.StudentID == id)
            .Select(x => new GetStudentResult{
                StudentId = x.StudentID,
                Name = $"{x.FirstName} {x.LastName}",
                Age = x.Age,
                MajorName = x.Major.MajorName
            })
            .FirstOrDefaultAsync();

            var response = new ApiResponse<GetStudentResult>
            {
                StatusCode = StatusCodes.Status200OK,
                RequestMethod = HttpContext.Request.Method,
                Data = studentData
            };
            
            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] UpdateStudentRequest request)
        {
            try
            {
                var studentData = await _context.Student
                .Where(x => x.StudentID == id)
                .FirstOrDefaultAsync();

                if(studentData == null){
                    throw new KeyNotFoundException("Student data not found");
                }

                studentData.FirstName = request.FirstName;
                studentData.LastName = request.LastName;
                studentData.Age = request.Age;
                studentData.MajorID = request.MajorID;

                _context.Student.Update(studentData);
                await _context.SaveChangesAsync();

                var response = new ApiResponse<string>{
                    StatusCode = StatusCodes.Status200OK,
                    RequestMethod = HttpContext.Request.Method,
                    Data = "Student is updated"
                };

                return Ok(response);
            }
            catch(Exception ex)
            {
                var response = new ApiResponse<string>{
                    StatusCode = StatusCodes.Status400BadRequest,
                    RequestMethod = HttpContext.Request.Method,
                    Data = ex.Message
                };
                return BadRequest(response);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id){
            try{
                var studentData = await _context.Student.Where(x => x.StudentID == id)
                .FirstOrDefaultAsync();

                if(studentData == null){
                    throw new KeyNotFoundException("Student data not found");
                }

                _context.Student.Remove(studentData);
                await _context.SaveChangesAsync();
                
                var response = new ApiResponse<string>{
                    StatusCode = StatusCodes.Status200OK,
                    RequestMethod = HttpContext.Request.Method,
                    Data = "Student is deleted"
                };

                return Ok(response);
            }
            catch(Exception ex){
                var response = new ApiResponse<string>{
                    StatusCode = StatusCodes.Status400BadRequest,
                    RequestMethod = HttpContext.Request.Method,
                    Data = ex.Message
                };
                return BadRequest(response);
            }
        }

    }
}
