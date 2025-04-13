using Application.CQRS.Recipes.Handlers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Application.CQRS.Products.Handlers.SearchProduct;
using static Application.CQRS.Recipes.Handlers.AddRecipe;
using static Application.CQRS.Recipes.Handlers.DeleteRecipe;
using static Application.CQRS.Recipes.Handlers.GetAllRecipes;
using static Application.CQRS.Recipes.Handlers.GetByCalorieRange;
using static Application.CQRS.Recipes.Handlers.GetByIngredient;
using static Application.CQRS.Recipes.Handlers.GetByMealType;
using static Application.CQRS.Recipes.Handlers.GetRecipeById;
using static Application.CQRS.Recipes.Handlers.SearchRecipeByName;
using static Application.CQRS.Recipes.Handlers.UpdateRecipe;

namespace FitnessProject.API.Controllers;

[Route("api/[controller]")]

[ApiController]
[Authorize]
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
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateRecipe(int id, [FromBody] UpdateRecipeCommand command)
    {
        if (id != command.Id)
            return BadRequest("ID uyuşmur.");

        var result = await _sender.Send(command);
        return result.IsSuccess ? Ok(result.Data) : BadRequest(result.Errors);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRecipe(int id)
    {
        var result = await _sender.Send(new DeleteRecipe.DeleteRecipeCommand { Id = id });
        if (!result.IsSuccess)
        {
            return NotFound(result.Errors);
        }
        return Ok(result.Data);
    }


    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string name)
    {
        var query = new SearchRecipeByNameQuery(name);
        var result = await _sender.Send(query);
        return Ok(result);
    }


    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _sender.Send(new GetAllRecipesQuery());
        return result.IsSuccess ? Ok(result.Data) : BadRequest(result.Errors);
    }

    [HttpGet("recipe/{recipeId}")]
    public async Task<IActionResult> GetRecipeById(int recipeId)
    {
        var query = new GetRecipeByIdQuery { Id = recipeId };
        var result = await _sender.Send(query);

        return result.IsSuccess ? Ok(result.Data) : NotFound(result.Errors);
    }


}

