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
}
