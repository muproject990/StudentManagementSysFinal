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
    public class CourseController : ControllerBase
    {
        private readonly IGenericService<Course> _courseService;
        private readonly IMapper _mapper;

        public CourseController(IGenericService<Course> courseService, IMapper mapper)
        {
            _courseService = courseService;
            _mapper = mapper;
        }



        [HttpGet]
        public async Task<IActionResult> GetAllCourse()
        {
            var result = await _courseService.GetAllAsync();
            if (result.IsSuccess)
            {

                return Ok(result.Data);
            }
            return BadRequest(new { Error = result.ErrorMessage });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCourse(int id)
        {
            var result = await _courseService.GetByIdAsync(id);
            if (result.IsSuccess)
                return Ok(result.Data);
            return NotFound(new { Error = result.ErrorMessage });
        }



        // [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateCourse([FromBody] CreateCourseDto createCourseDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            Course course = _mapper.Map<Course>(createCourseDto);


            var result = await _courseService.AddAsync(course);
            if (result.IsSuccess)
                return CreatedAtAction(nameof(GetCourse), new { id = result.Data.CourseID }, result.Data);

            return BadRequest(new { Error = result.ErrorMessage });
        }



        // [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCourse(int id, [FromBody] CreateCourseDto updateCourseDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            var getResult = await _courseService.GetByIdAsync(id);
            if (!getResult.IsSuccess)
                return NotFound(new { Error = getResult.ErrorMessage });

            Course existingCourse = getResult.Data;



            _mapper.Map(updateCourseDto, existingCourse);


            var updateResult = await _courseService.UpdateAsync(existingCourse);
            if (updateResult.IsSuccess)
                return Ok(new { Message = "Course updated successfully." });

            return BadRequest(new { Error = updateResult.ErrorMessage });
        }




        // [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var result = await _courseService.DeleteAsync(id);
            if (result.IsSuccess)
                return Ok(new { Message = "Course deleted successfully." });
            return NotFound(new { Error = result.ErrorMessage });
        }
    }
}
