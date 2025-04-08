using MediatR;
using Microsoft.AspNetCore.Mvc;
using static Application.CQRS.Recipes.Handlers.AddRecipe;
using static Application.CQRS.Recipes.Handlers.GetByCalorieRange;
using static Application.CQRS.Recipes.Handlers.GetByIngredient;
using static Application.CQRS.Recipes.Handlers.GetByMealType;

namespace FitnessProject.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RecipeController(ISender sender) : ControllerBase
{
    private readonly ISender _sender = sender;

    [HttpGet("by-calories")]
    public async Task<IActionResult> GetByCalorieRange([FromQuery] int minCalories, [FromQuery] int maxCalories)
    {
        var query = new GetByCalorieRangeQuery(minCalories, maxCalories);
        var result = await _sender.Send(query);

        if (!result.IsSuccess)
        {
            return BadRequest(result.Errors);
        }

        return Ok(result.Data);
    }

    [HttpGet("by-ingredient")]
    public async Task<IActionResult> GetByIngredient([FromQuery] string ingredient)
    {
        var query = new GetByIngredientQuery(ingredient);
        var result = await _sender.Send(query);

        if (!result.IsSuccess)
        {
            return BadRequest(result.Errors);
        }

        return Ok(result.Data);
    }

    [HttpGet("by-mealtype")]
    public async Task<IActionResult> GetByMealType([FromQuery] string mealType)
    {
        var query = new GetByMealTypeQuery(mealType);
        var result = await _sender.Send(query);

        if (!result.IsSuccess)
        {
            return BadRequest(result.Errors);
        }

        return Ok(result.Data);
    }

    [HttpPost("add")]
    public async Task<IActionResult> AddRecipe([FromBody] AddRecipeCommand command)
    {
        var result = await _sender.Send(command);
        if (!result.IsSuccess)
        {
            return BadRequest(result.Errors);
        }
        return CreatedAtAction(nameof(GetByCalorieRange), new { id = result.Data.Id }, result.Data);
    }

}

