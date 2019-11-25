namespace _01.Prototype
{
    public class StartUp
    {
        public static void Main()
        {
            var sandwichMenu = new SandwichMenu();

            sandwichMenu["BLT"] = new Sandwich("Wheat", "Bacon", "", "Lettuce, Tomato");
            sandwichMenu["PB&J"] = new Sandwich("White", "", "", "Peanut Butter, Jelly");
            sandwichMenu["Turkey"] = new Sandwich("Rye", "Turkey", "Swiss", "Lettuce, Onion, Tomato");
            
            sandwichMenu["LoadedBLT"] = new Sandwich("Wheat", "Turkey, Bacon", "American", "Lettuce, Onion, Tomato, Olives");
            sandwichMenu["ThreeMeatCombo"] = new Sandwich("Rye", "Turkey, Ham, Salami", "Provolone", "Lettuce, Onion");
            sandwichMenu["Vegetarian"] = new Sandwich("Wheat", "", "", "Lettuce, Onion, Tomato, Olives, Spinach");

            var firstSandwich = sandwichMenu["BLT"].Clone() as Sandwich;
            var secondSandwich = sandwichMenu["ThreeMeatCombo"].Clone() as Sandwich;
            var thirtSandwich = sandwichMenu["Vegetarian"].Clone() as Sandwich;
        }
    }
}
