using Application.CQRS.WorkoutPlan.ResponseDto;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static Application.CQRS.WorkoutPlan.Handlers.GenerateWorkoutHandler;

namespace FitnessProject.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class WorkoutPlanController(ISender sender) : ControllerBase
{
    private readonly ISender _sender = sender;

    [HttpPost("generate-workout")]
    public async Task<IActionResult> Generate([FromBody] WorkoutPlanDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var command = new GenerateWorkoutCommand(dto);
        var result = await _sender.Send(command);

        return Ok(new
        {
            message = "Workout plan created succesfully",
            content = result
        });
    }
}
