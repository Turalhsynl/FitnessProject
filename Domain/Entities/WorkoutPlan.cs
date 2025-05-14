using Domain.BaseEntities;

namespace Domain.Entities;

public class WorkoutPlan : BaseEntity
{
    public int UserId { get; set; }
    public string FitnessCategory { get; set; }
    public int Age { get; set; }
    public string Gender { get; set; }
    public string Goal { get; set; }
    public string Level { get; set; }
    public int DaysPerWeek { get; set; }
    public string BodyType { get; set; }
    public string DreamBody { get; set; }
    public string TargetZone { get; set; }
    public string SleepTime { get; set; }
    public double Height { get; set; }
    public double Weight { get; set; }
    public string AiGeneratedContent { get; set; }
    public User User { get; set; }
}
