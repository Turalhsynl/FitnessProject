using Domain.BaseEntities;

namespace Domain.Entities;

public class WorkoutPlan : BaseEntity
{
    public int UserId { get; set; }
    public string Goal { get; set; }
    public string Level { get; set; }
    public int DaysPerWeek { get; set; }
    public string Gender { get; set; }
    public string AiGeneratedContent { get; set; }

    public User User { get; set; }
}
