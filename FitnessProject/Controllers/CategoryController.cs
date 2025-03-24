using Application.CQRS.Categories.Handlers;
using Application.CQRS.Categories.ResponseDto;
using Application.CQRS.Users.Handlers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repository.Common;
using static Application.CQRS.Categories.Handlers.Add;
using static Application.CQRS.Categories.Handlers.GetById;
using static Application.CQRS.Categories.Handlers.UpdateCategory;

namespace FitnessProject.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CategoryController(ISender sender) : ControllerBase
{
    private readonly ISender _sender = sender;

    [HttpPost("CreateCategory")]
    public async Task<IActionResult> CreateCategory([FromBody] AddCommand request)
    {
        return Ok(await _sender.Send(request));
    }

    [HttpGet("GetAllCategory")]
    public async Task<IActionResult> GetAllCategories()
    {
        var result = await _sender.Send(new GetAllCategoryQuery());

        if (!result.IsSuccess)
        {
            return NotFound(result.Errors);
        }

        return Ok(result.Data);
    }

    [HttpGet("GetByIdCategory")]
    public async Task<IActionResult> GetByIdCategory([FromQuery] GetByIdCategoryQuery request)
    {
        return Ok(await _sender.Send(request));
    }

    [HttpPut("UpdateCategory")]
    public async Task<IActionResult> UpdateCategory([FromBody] UpdateCategoryCommand request)
    {
        return Ok(await _sender.Send(request));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategory(int id, [FromQuery] int deletedBy)
    {
        return Ok(await _sender.Send(new DeleteCategory.DeleteCategoryCommand { Id = id, DeletedBy = deletedBy }));
    }
}
