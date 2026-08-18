using LoggerCategory.Services;
using Microsoft.AspNetCore.Mvc;

namespace LoggerCategory.Controllers;

[ApiController]
[Route("api/process")]
public class ProcessController(ProcessService processService) : ControllerBase
{

    [HttpGet("{ProcessId:Guid}")]
    public IActionResult Process(Guid ProcessId)
    {
        processService.Process(ProcessId);
        return Ok(new { ProcessId = ProcessId, Status = "Processed" });
    }
}