namespace AdventOfCode2020.Solver;

internal partial class Day21 : BaseSolver
{
    public override string PuzzleTitle { get; } = "Allergen Assessment";

    private sealed class Food
    {
        public List<string> Ingredients { get; init; }
        public List<string> Allergens { get; init; }

        public Food(string label)
        {
            string[] parts = label.Trim(')').Split("(contains");
            Ingredients = [.. parts[0].Trim().Split(" ", StringSplitOptions.RemoveEmptyEntries)];
            Allergens = [.. parts[1].Trim().Split(", ", StringSplitOptions.RemoveEmptyEntries)];
        }
    }

    private readonly List<Food> _allFood = [];

    public override string GetSolution1(bool isChallenge)
    {
        ExtractData();

        List<(string allergen, string ingredient)> allergenIngredientPair = IdentifyAllergenIngredients();
        List<string> allIngredients = _allFood.SelectMany(p => p.Ingredients).Distinct()
            .Where(a => !allergenIngredientPair.ConvertAll(p => p.ingredient).Contains(a)).ToList();

        return allIngredients.Sum(i => _allFood.Count(f => f.Ingredients.Contains(i))).ToString();
    }

    public override string GetSolution2(bool isChallenge)
    {
        ExtractData();

        List<(string allergen, string ingredient)> allergenIngredientPair = IdentifyAllergenIngredients();

        return string.Join(",", allergenIngredientPair.OrderBy(i => i.allergen).Select(i => i.ingredient));
    }

    private List<(string allergen, string ingredient)> IdentifyAllergenIngredients()
    {
        // Get all allergen
        List<string> allAllergens = _allFood.SelectMany(p => p.Allergens).Distinct().ToList();

        // Create a dictionary of all allergens and possible Ingredient list linked to them
        Dictionary<string, List<string>> AllergenIngredientDic = allAllergens.ToDictionary(i => i, i => new List<string>());
        foreach (string allergen in AllergenIngredientDic.Keys)
        {
            // In multiple step for clarity. First we get all food having requested allergen
            List<List<string>> allFoodWithAllergen = [.. _allFood.FindAll(f => f.Allergens.Contains(allergen)).ConvertAll<List<string>>(f => [.. f.Ingredients])];

            // Then we keep only ingredient who are present in all food
            List<string> commonIngredients = allFoodWithAllergen.Aggregate((List<string> acc, List<string> value) => [.. acc.Intersect(value)]);

            // Save result
            AllergenIngredientDic[allergen].AddRange(commonIngredients);
        }

        // Create list of allergen and linked ingredient
        List<(string allergen, string ingredient)> result = [];
        while (AllergenIngredientDic.Values.Any(i => i.Count > 0))
        {
            // Find allergen with only one ingredient
            KeyValuePair<string, List<string>> kvp = AllergenIngredientDic.First(kvp => kvp.Value.Count == 1);

            // Save it to result
            string ingredient = kvp.Value[0];
            result.Add((kvp.Key, ingredient));

            // We remove this ingredient from all remaining items
            foreach (KeyValuePair<string, List<string>> kvp2 in AllergenIngredientDic)
            {
                kvp2.Value.Remove(ingredient);
            }
        }

        // Done
        return result;
    }

    private void ExtractData()
    {
        _allFood.Clear();
        foreach (string line in _puzzleInput)
        {
            _allFood.Add(new(line));
        }
    }
}