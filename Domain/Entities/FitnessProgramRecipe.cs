using Domain.BaseEntities;

namespace Domain.Entities;

public class FitnessProgramRecipe : BaseEntity
{
    public int FitnessProgramId { get; set; }
    public FitnessProgram FitnessProgram { get; set; }

    public int RecipeId { get; set; }
    public Recipe Recipe { get; set; }
}
