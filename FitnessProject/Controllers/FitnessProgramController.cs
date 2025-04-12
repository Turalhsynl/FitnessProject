using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Application.CQRS.FitnessPrograms.Handlers.CreateFitnessProgram;
using static Application.CQRS.FitnessPrograms.Handlers.DeleteFitnessProgram;
using static Application.CQRS.FitnessPrograms.Handlers.GetAllFitnessProgram;
using static Application.CQRS.FitnessPrograms.Handlers.GetFitnessProgramById;
using static Application.CQRS.FitnessPrograms.Handlers.UpdateFitnessProgram;

namespace FitnessProject.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class FitnessProgramController(ISender sender) : ControllerBase
{
    private readonly ISender _sender = sender;

    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAllFitnessPrograms()
    {
        var result = await _sender.Send(new GetAllFitnessProgramsQuery());

        if (!result.IsSuccess)
        {
            return NotFound(result.Errors);
        }

        return Ok(result.Data);
    }

    [HttpGet("GetById")]
    public async Task<IActionResult> GetFitnessProgramById([FromQuery] int id)
    {
        var result = await _sender.Send(new GetFitnessProgramByIdQuery { Id = id });

        if (!result.IsSuccess)
        {
            return NotFound(result.Errors);
        }

        return Ok(result.Data);
    }

    [HttpPost("Create")]
    public async Task<IActionResult> CreateFitnessProgram([FromBody] CreateFitnessProgramCommand request)
    {
        return Ok(await _sender.Send(request));
    }

    [HttpPut("Update")]
    public async Task<IActionResult> UpdateFitnessProgram([FromBody] UpdateFitnessProgramCommand request)
    {
        return Ok(await _sender.Send(request));
    }

    [HttpDelete("Delete")]
    public async Task<IActionResult> DeleteFitnessProgram([FromQuery] int id)
    {
        var request = new DeleteFitnessProgramCommand { Id = id };
        return Ok(await _sender.Send(request));
    }
}
