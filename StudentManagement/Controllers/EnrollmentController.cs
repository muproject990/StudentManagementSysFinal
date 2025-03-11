using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using AutoMapper;
using StudentManagement.App.DTOs;
using StudentManagement.Domain.Entities;
using StudentManagement.App.services;
using Microsoft.AspNetCore.Authorization;

namespace StudentManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EnrollmentController : ControllerBase
    {
        private readonly IGenericService<Enrollment> _enrollmentService;
        private readonly IMapper _mapper;

        public EnrollmentController(IGenericService<Enrollment> enrollmentService, IMapper mapper)
        {
            _enrollmentService = enrollmentService;
            _mapper = mapper;
        }



        [HttpGet]
        public async Task<IActionResult> GetAllEnrollment()
        {
            var result = await _enrollmentService.GetAllAsync();
            if (result.IsSuccess)
            {

                return Ok(result.Data);
            }
            return BadRequest(new { Error = result.ErrorMessage });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEnrollment(int id)
        {
            var result = await _enrollmentService.GetByIdAsync(id);
            if (result.IsSuccess)
                return Ok(result.Data);
            return NotFound(new { Error = result.ErrorMessage });
        }



        // [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateEnrollment([FromBody] CreateEnrollmentDto createEnrollmentDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            Enrollment enrollment = _mapper.Map<Enrollment>(createEnrollmentDto);


            var result = await _enrollmentService.AddAsync(enrollment);
            if (result.IsSuccess)
                return CreatedAtAction(nameof(GetEnrollment), new { id = result.Data.EnrollmentID }, result.Data);

            return BadRequest(new { Error = result.ErrorMessage });
        }



        // [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEnrollment(int id, [FromBody] CreateEnrollmentDto updateEnrollmentDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            var getResult = await _enrollmentService.GetByIdAsync(id);
            if (!getResult.IsSuccess)
                return NotFound(new { Error = getResult.ErrorMessage });

            Enrollment existingEnrollment = getResult.Data;



            _mapper.Map(updateEnrollmentDto, existingEnrollment);


            var updateResult = await _enrollmentService.UpdateAsync(existingEnrollment);
            if (updateResult.IsSuccess)
                return Ok(new { Message = "Enrollment updated successfully." });

            return BadRequest(new { Error = updateResult.ErrorMessage });
        }




        // [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEnrollment(int id)
        {
            var result = await _enrollmentService.DeleteAsync(id);
            if (result.IsSuccess)
                return Ok(new { Message = "Enrollment deleted successfully." });
            return NotFound(new { Error = result.ErrorMessage });
        }
    }
}
