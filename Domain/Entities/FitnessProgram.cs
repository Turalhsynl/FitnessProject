using Domain.BaseEntities;
using Domain.Entities;

public class FitnessProgram : BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string Level { get; set; }
    public int DurationInWeeks { get; set; }
    public string Gender { get; set; }
    public decimal Price { get; set; }
    public string VideoUrl { get; set; }
    public string ImageUrl { get; set; }
    public ICollection<FitnessProgramRecipe> FitnessProgramRecipes { get; set; }
}
