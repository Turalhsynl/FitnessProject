using Application.CQRS.MembershipPlans.Handlers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FitnessProject.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MembershipPlanController(ISender sender) : ControllerBase
{
    private readonly ISender _sender = sender;

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _sender.Send(new GetMembershipPlanById.Query { Id = id });
        return result.IsSuccess ? Ok(result) : NotFound(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _sender.Send(new GetAllMembershipPlans.Query());
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateMembershipPlan.CreateMembershipPlanCommand command)
    {
        var result = await _sender.Send(command);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateMembershipPlan.UpdateMembershipPlanCommand command)
    {
        if (id != command.Id)
            return BadRequest("Id mismatch between URL and body.");

        var result = await _sender.Send(command);
        return result.IsSuccess ? Ok(result) : NotFound(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _sender.Send(new DeleteMembershipPlan.DeleteMembershipPlanCommand { Id = id });
        return result.IsSuccess ? Ok(result) : NotFound(result);
    }

    [HttpGet("exists/{id}")]
    public async Task<IActionResult> Exists(int id)
    {
        var result = await _sender.Send(new CheckMembershipPlanExists.CheckMembershipPlanExistsQuery { Id = id });
        return Ok(result);
    }
}