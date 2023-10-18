using API.Utility;
using Data.Exceptions;
using Data.Models;
using Data.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MalfunctionController : ControllerBase
    {
        private readonly ILogger<MalfunctionController> _logger;
        private readonly MalfunctionRepository _malfunctionRepository;

        public MalfunctionController(ILogger<MalfunctionController> logger,MalfunctionRepository malfunctionRepository)
        {
            _logger = logger;
            _malfunctionRepository = malfunctionRepository;
        }

        [HttpGet]
        [Authorize(Roles = Roles.ADMIN)]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("Fetching all malfunctions");

            try
            {
                var malfunctions = await _malfunctionRepository.GetAll();

                return malfunctions.Any() ? Ok(malfunctions) : NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching malfunctions");
                return StatusCode(500, "An error occurred while processing your request");
            }
        }

        [HttpGet("{malfunctionId}")]
        public async Task<IActionResult> GetById(int malfunctionId)
        {
            _logger.LogInformation($"Fetching malfunction with ID {malfunctionId}");

            try
            {
                var malfunction = await _malfunctionRepository.GetById(malfunctionId);
                return malfunction is null ? NoContent() : Ok(malfunction);
            }
            catch (ModelNotFoundException)
            {
                return BadRequest($"A malfunction with ID {malfunctionId} was not found");
            }
            catch (ArgumentOutOfRangeException)
            {
                return BadRequest("Invalid ID provided");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while fetching a malfunction.");
                return StatusCode(500, "An unexpected error occurred while processing your request.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Malfunction malfunction)
        {
            if (malfunction is null)
                return BadRequest("Invalid body content provided");

            _logger.LogInformation("Creating malfunction.");

            try
            {
                var model = await _malfunctionRepository.Create(malfunction);
                return Created($"Malfunction/{model.MalfunctionId}", model);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "An error occurred while creating the malfunction in the database.");
                return BadRequest($"Malfunction not created.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while creating the malfunction");
                return StatusCode(500, "An unexpected error occurred while processing your request.");
            }

        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] Malfunction malfunction)
        {
            if (malfunction is null)
            {
                return BadRequest("Invalid body content provided");
            }

            _logger.LogInformation($"Updating malfunction with ID: {malfunction.MalfunctionId}");

            try
            {
                return Ok(await _malfunctionRepository.Update(malfunction));
            }
            catch (ModelNotFoundException)
            {
                return BadRequest($"A malfunction with ID {malfunction.MalfunctionId} was not found.");
            }
            catch (DbUpdateConcurrencyException)
            {
                return Conflict($"Malfunction with ID {malfunction.MalfunctionId} was updated by another user. Retrieve the latest version and try again.");
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "An error occurred while updating the malfunction in the database.");
                return StatusCode(500, "An error occurred while updating the malfunction in the database.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while updating a malfunction.");
                return StatusCode(500, "An unexpected error occurred while processing your request.");
            }
        }

        [HttpPost("{malfunctionId}")]
        public async Task<IActionResult> Delete(int malfunctionId)
        {
            if (malfunctionId <= 0)
                return BadRequest("Invalid ID provided");

            _logger.LogInformation($"Deleting malfunction with ID: {malfunctionId}");

            try
            {
                await _malfunctionRepository.Delete(malfunctionId);
                return Ok();
            }
            catch (ModelNotFoundException)
            {
                return BadRequest($"A malfunction with ID {malfunctionId} was not found");
            }
            catch (ArgumentOutOfRangeException)
            {
                return BadRequest("Invalid ID provided");
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the malfunction from the database.");
                return StatusCode(500, "An error occurred while deleting the malfunction from the database.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while deleting the malfunction.");
                return StatusCode(500, "An unexpected error occurred while processing your request.");
            }
        }
    }
}
