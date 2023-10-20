using Data.Exceptions;
using Data.Interfaces;
using Data.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly ILogger _logger;

        public DepartmentController(ILogger logger, IDepartmentRepository departmentRepository)
        {
            _logger = logger;
            _departmentRepository = departmentRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("Fetching all departments");

            try
            {
                var departments = await _departmentRepository.GetAll();

                return departments.Any() ? Ok(departments) : NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured while fetching departments");
                return StatusCode(500, "An error occurred while processing your request");
            }

        }

        [HttpGet("{departmentId}")]
        public async Task<IActionResult> GetById(int departmentId)
        {
            _logger.LogInformation($"Fetching department with ID {departmentId}");
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while fetching a department.");
                return StatusCode(500, "An unexpected error occurred while processing your request.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Department department)
        {
            _logger.LogInformation("Creating Department");
            if (department is null)
            {
                _logger.LogWarning("Invalid body content provided");
                return BadRequest("Invalid body content provided");
            }

            try
            {
                var model = await _departmentRepository.Create(department);
                return Created($"Department/{model.Id}", model);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError("An update concurrency occured while trying to create department", ex);
                return Conflict(await _departmentRepository.GetById(department.Id));
            }
            catch (DbUpdateException)
            {
                return BadRequest("Department not created");
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] Department department)
        {
            if (department is null)
                return BadRequest("Invalid body content provided");

            _logger.LogInformation($"Updating department with ID: {department.Id}");

            try
            {
                return Ok(await _departmentRepository.Update(department));
            }
            catch (ModelNotFoundException)
            {
                return BadRequest($"A department with ID {department.Id} was not found.");
            }
            catch (DbUpdateConcurrencyException)
            {
                return Conflict($"Department with ID {department.Id} was updated by another user. Retrieve the latest version and try again.");
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "An error occurred while updating the department in the database.");
                return StatusCode(500, "An error occurred while updating the department in the database.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while updating a department.");
                return StatusCode(500, "An unexpected error occurred while processing your request.");
            }
        }

        [HttpPost("{departmentId}")]
        public async Task<IActionResult> Delete(int departmentId)
        {
            if (departmentId <= 0)
                return BadRequest("Invalid ID provided");

            _logger.LogInformation($"Deleting department with ID: {departmentId}");

            try
            {
                await _departmentRepository.Delete(departmentId);
                return Ok();
            }
            catch (ModelNotFoundException)
            {
                return BadRequest($"A department with ID {departmentId} was not found");
            }
            catch (ArgumentOutOfRangeException)
            {
                return BadRequest("Invalid ID provided");
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the department from the database.");
                return StatusCode(500, "An error occurred while deleting the department from the database.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while deleting the department.");
                return StatusCode(500, "An unexpected error occurred while processing your request.");
            }
        }
    }
}
