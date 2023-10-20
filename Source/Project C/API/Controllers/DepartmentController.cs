using Data.Exceptions;
using Data.Interfaces;
using Data.Models;

using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentRepository _departmentRepository;

        public DepartmentController(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var departments = await _departmentRepository.GetAll();
            if (departments.Count == 0)
                return NoContent();

            return Ok(departments);
        }

        [HttpGet("{departmentId}")]
        public async Task<IActionResult> GetById(int departmentId)
        {
            try
            {
                var department = await _departmentRepository.GetById(departmentId);
                return Ok(department);
            }
            catch (ModelNotFoundException)
            {
                return BadRequest($"A department with ID {departmentId} was not found");
            }
            catch (ArgumentOutOfRangeException)
            {
                return BadRequest("Invalid ID provided");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Department department)
        {
            if (department is null)
                return BadRequest("Invalid body content provided");

            var model = await _departmentRepository.Create(department);
            return Created($"Department/{model.Id}", model);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] Department department)
        {
            if (department is null)
                return BadRequest("Invalid body content provided");

            try
            {
                return Ok(await _departmentRepository.Update(department));
            }
            catch (ModelNotFoundException)
            {
                return BadRequest($"A department with ID {department.Id} was not found.");
            }
        }

        [HttpPost("{departmentId}")]
        public async Task<IActionResult> Delete(int departmentId)
        {
            if (departmentId <= 0)
                return BadRequest("Invalid ID provided");

            try
            {
                await _departmentRepository.Delete(departmentId);
                return Ok();
            }
            catch (ModelNotFoundException)
            {
                return BadRequest($"A department with ID {departmentId} was not found");
            }
        }
    }
}
