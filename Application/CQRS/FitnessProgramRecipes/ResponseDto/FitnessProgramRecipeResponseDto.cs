namespace Application.CQRS.FitnessProgramRecipes.ResponseDto;

public class FitnessProgramRecipeResponseDto
{
    public int RecipeId { get; set; }
    public string RecipeName { get; set; }
    public string Description { get; set; }
    public int ImageId { get; set; }
    public int Calories { get; set; }
    public string MealType { get; set; }
}
