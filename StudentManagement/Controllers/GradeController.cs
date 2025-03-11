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
    public class GradeController : ControllerBase
    {
        private readonly IGenericService<Grade> _gradeService;
        private readonly IMapper _mapper;

        public GradeController(IGenericService<Grade> gradeService, IMapper mapper)
        {
            _gradeService = gradeService;
            _mapper = mapper;
        }



        [HttpGet]
        public async Task<IActionResult> GetAllGrade()
        {
            var result = await _gradeService.GetAllAsync();
            if (result.IsSuccess)
            {

                return Ok(result.Data);
            }
            return BadRequest(new { Error = result.ErrorMessage });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetGrade(int id)
        {
            var result = await _gradeService.GetByIdAsync(id);
            if (result.IsSuccess)
                return Ok(result.Data);
            return NotFound(new { Error = result.ErrorMessage });
        }



        // [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateGrade([FromBody] CreateGradeDto createGradeDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            Grade grade = _mapper.Map<Grade>(createGradeDto);


            var result = await _gradeService.AddAsync(grade);
            if (result.IsSuccess)
                return CreatedAtAction(nameof(GetGrade), new { id = result.Data.GradeID }, result.Data);

            return BadRequest(new { Error = result.ErrorMessage });
        }



        // [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGrade(int id, [FromBody] CreateGradeDto updateGradeDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            var getResult = await _gradeService.GetByIdAsync(id);
            if (!getResult.IsSuccess)
                return NotFound(new { Error = getResult.ErrorMessage });

            Grade existingGrade = getResult.Data;



            _mapper.Map(updateGradeDto, existingGrade);


            var updateResult = await _gradeService.UpdateAsync(existingGrade);
            if (updateResult.IsSuccess)
                return Ok(new { Message = "Grade updated successfully." });

            return BadRequest(new { Error = updateResult.ErrorMessage });
        }




        // [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGrade(int id)
        {
            var result = await _gradeService.DeleteAsync(id);
            if (result.IsSuccess)
                return Ok(new { Message = "Grade deleted successfully." });
            return NotFound(new { Error = result.ErrorMessage });
        }
    }
}
