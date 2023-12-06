using API.Utility;

using Data.Dtos;
using Data.Exceptions;
using Data.Interfaces;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class MachineController : ControllerBase
{
    private readonly IMachineRepository _machineRepository;
    private readonly ILogger<MachineController> _logger;

    public MachineController(ILogger<MachineController> logger, IMachineRepository machineRepository)
    {
        _logger = logger;
        _machineRepository = machineRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok();   
    }
}
