using Application.CQRS.Categories.Handlers;
using Application.CQRS.Users.Handlers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Application.CQRS.Categories.Handlers.Add;

namespace FitnessProject.API.Controllers;

[Route("api/[controller]")]
[ApiController]
//[Authorize]
public class CategoryController(ISender sender) : ControllerBase
{
    private readonly ISender _sender = sender;

    [HttpPost]
    public async Task<IActionResult> CreateCategory([FromBody] AddCommand request)
    {
        return Ok(await _sender.Send(request));
    }

    [HttpGet]
    public async Task<IActionResult> GetAllCategories()
    {
        var result = await _sender.Send(new GetAllCategoryQuery());

        if (!result.IsSuccess)
        {
            return NotFound(result.Errors);
        }

        return Ok(result.Data);
    }
}
