using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using AutoMapper;
using StudentManagement.App.DTOs;
using StudentManagement.Domain.Entities;
using StudentManagement.App.services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using StudentManagementSystem.Application.Handlers;
using StudentManagement.App.Queries;
using Microsoft.AspNetCore.Http.HttpResults;

namespace StudentManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController(IGenericService<Student> studentService, IMapper mapper
    , GetTopThreeStudentsHandler getTopThreeStudentsHandler
    )
     : ControllerBase
    {
        private readonly IGenericService<Student> _studentService = studentService;
        private readonly IMapper _mapper = mapper;

        private readonly GetTopThreeStudentsHandler _getTopThreeStudentsHandler = getTopThreeStudentsHandler;

        // GET: api/Students
        // Returns all students (optionally,  can create a StudentDto to control what data is exposed)



        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAllStudents()
        {
            var result = await _studentService.GetAllAsync();
            if (result.IsSuccess)
            {
                // Optionally, map the domain entity to a DTO if desired.
                // For simplicity, we are returning the domain entity.
                return Ok(result.Data);
            }
            return BadRequest(new { Error = result.ErrorMessage });
        }


        [HttpGet("TopThree")]
        public async Task<IActionResult> GetTopThreeStudent()
        {
            var query = new GetTopThreeStudentsQuery();
            var res = await _getTopThreeStudentsHandler.HandleAsync(query);
            if (res.IsSuccess)
            {
                return Ok(res.Data);
            }
            return BadRequest(new { Error = res.ErrorMessage });

        }

        // GET: api/Students/{id}
        // Returns a single student by its ID.
        [HttpGet("{id}")]
        public async Task<IActionResult> GetStudent(int id)
        {
            var result = await _studentService.GetByIdAsync(id);
            if (result.IsSuccess)
                return Ok(result.Data);
            return NotFound(new { Error = result.ErrorMessage });
        }

        // POST: api/Students
        // Uses CreateStudentDto so that only the required fields are provided by the client.
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateStudent([FromBody] CreateStudentDto createStudentDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Map DTO to domain entity.
            Student student = _mapper.Map<Student>(createStudentDto);

            // Call the generic service to add the student.
            var result = await _studentService.AddAsync(student);
            if (result.IsSuccess)
                return CreatedAtAction(nameof(GetStudent), new { id = result.Data.StudentID }, result.Data);

            return BadRequest(new { Error = result.ErrorMessage });
        }

        // PUT: api/Students/{id}
        // Updates an existing student.  could use a separate DTO (e.g., UpdateStudentDto) if needed.
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStudent(int id, [FromBody] CreateStudentDto updateStudentDto)  // reusing CreateStudentDto for brevity
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Retrieve the existing student from the database.
            var getResult = await _studentService.GetByIdAsync(id);
            if (!getResult.IsSuccess)
                return NotFound(new { Error = getResult.ErrorMessage });

            Student existingStudent = getResult.Data;

            // Map the updated values from the DTO into the existing student.
            // This may be done manually or via AutoMapper; here we let AutoMapper map updated fields.
            _mapper.Map(updateStudentDto, existingStudent);

            // Call the generic service to update the student.
            var updateResult = await _studentService.UpdateAsync(existingStudent);
            if (updateResult.IsSuccess)
                return Ok(new { Message = "Student updated successfully." });

            return BadRequest(new { Error = updateResult.ErrorMessage });
        }

        // DELETE: api/Students/{id}
        // Deletes the student

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var result = await _studentService.DeleteAsync(id);
            if (result.IsSuccess)
                return Ok(new { Message = "Student deleted successfully." });
            return NotFound(new { Error = result.ErrorMessage });
        }
    }
}
