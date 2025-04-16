namespace Application.CQRS.MembershipPlans.ResponseDto;

public class MembershipPlanDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int MaxProgramsAllowed { get; set; }
}
