using Application.CQRS.FitnessProgramRecipes.Handler;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Application.CQRS.FitnessProgramRecipes.Handler.DeleteFitnessProgramRecipe;

namespace FitnessProject.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class FitnessProgramRecipeController(ISender sender) : ControllerBase
{
    private readonly ISender _sender = sender;

    [HttpPost("Add")]
    public async Task<IActionResult> AddAsync([FromBody] AddFitnessProgramRecipeCommand request)
    {
        var result = await _sender.Send(request);

        if (!result.IsSuccess)
            return BadRequest(result.Errors);

        return Ok(result.Data);
    }

    [HttpDelete("DeleteRelationship")]
    public async Task<IActionResult> DeleteRelationshipAsync([FromBody] DeleteFitnessProgramRecipeCommand request)
    {
        var result = await _sender.Send(request);

        if (!result.IsSuccess)
        {
            return BadRequest(result.Errors);
        }

        return Ok("Fitness program-recipe successfully deleted.");
    }
}
