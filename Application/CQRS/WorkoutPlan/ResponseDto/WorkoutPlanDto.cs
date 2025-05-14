namespace Application.CQRS.WorkoutPlan.ResponseDto;

public class WorkoutPlanDto
{
    public int UserId { get; set; }
    public string Goal { get; set; }
    public string Level { get; set; }
    public int DaysPerWeek { get; set; }
    public string Gender { get; set; }
}
