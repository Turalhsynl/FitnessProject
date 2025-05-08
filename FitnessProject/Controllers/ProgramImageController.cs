using Application.Abstractions;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace FitnessProject.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProgramImageController(IProgramService programService) : ControllerBase
{
    private readonly IProgramService _programService = programService;

    [HttpPost("upload-image")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UploadProgramImage([FromForm] UploadProgramImageDto dto)
    {
        var result = await _programService.UploadProgramImageAsync(dto);
        return Ok(new { fileName = result });
    }
}
    
