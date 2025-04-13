using Domain.BaseEntities;

namespace Domain.Entities;

public class Recipe : BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }

    public string Ingredients { get; set; }
    public string Instructions { get; set; }

    public string ImageUrl { get; set; }

    public int Calories { get; set; }

    public string MealType { get; set; }

    public ICollection<FitnessProgramRecipe> FitnessProgramRecipes { get; set; }
}
