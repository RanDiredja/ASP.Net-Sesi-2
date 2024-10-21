using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using MyUniversity.Data;
using MyUniversity.Models;
using MyUniversity.Models.Request;
using MyUniversity.Models.Result;


namespace MyUniversity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        public StudentController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: api/<StudentController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetStudentResult>>> Get()
        {
            var listStudent = await _dbContext.Student
                .Include(x => x.Major)
                .Select(x => new GetStudentResult
                {
                    StudentID = x.StudentID,
                    Name = $"{x.FirstName} {x.LastName}",
                    Age = x.Age,
                    MajorName = x.Major.MajorName
                })
                .ToListAsync();

            return listStudent;
        }

        // GET api/<StudentController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GetStudentResult>> Get(string id)
        {
            var studentData = await _dbContext.Student.Where(x => x.StudentID == id)
                .Select(x => new GetStudentResult
                {
                    StudentID = x.StudentID,
                    Name = $"{x.FirstName} {x.LastName}",
                    Age = x.Age,
                    MajorName = x.Major.MajorName
                })
                .FirstOrDefaultAsync();

            if (studentData == null)
            {
                return BadRequest();
            }

            return studentData;
        }

        // POST api/<StudentController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateStudentRequest request)
        {
            var isStudentExist = await _dbContext.Student.Where(x => x.StudentID == request.StudentID).AnyAsync();

            if (isStudentExist)
            {
                return BadRequest("Student already exist");
            }

            var isMajorExist = await _dbContext.Major.Where(x => x.MajorID == request.MajorID).AnyAsync();
            if (!isMajorExist)
            {
                return BadRequest("Major not found");
            }

            var student = new Student
            {
                StudentID = request.StudentID,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Age = request.Age,
                MajorID = request.MajorID
            };

            _dbContext.Student.Add(student);
            await _dbContext.SaveChangesAsync();

            return Ok();
        }

        // PUT api/<StudentController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] UpdateStudentRequest request)
        {
            var studentData = await _dbContext.Student.Where(x => x.StudentID == id).FirstOrDefaultAsync();

            if (studentData == null)
            {
                return BadRequest("Student data not found");
            }

            var isMajorExist = await _dbContext.Major.Where(x => x.MajorID == request.MajorID).AnyAsync();
            if (!isMajorExist)
            {
                return BadRequest("Major not found");
            }

            studentData.FirstName = request.FirstName;
            studentData.LastName = request.LastName;
            studentData.Age = request.Age;
            studentData.MajorID = request.MajorID;

            _dbContext.Student.Update(studentData);
            await _dbContext.SaveChangesAsync();

            return Ok();
        }

        // DELETE api/<StudentController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var studentData = await _dbContext.Student.Where(x => x.StudentID == id).FirstOrDefaultAsync();

            if (studentData == null)
            {
                return BadRequest("Student data not found");
            }

            _dbContext.Student.Remove(studentData);
            await _dbContext.SaveChangesAsync();

            return Ok();

        }
    }
}
