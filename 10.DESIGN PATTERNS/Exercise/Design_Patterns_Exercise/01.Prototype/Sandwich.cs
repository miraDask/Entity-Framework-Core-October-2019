namespace _01.Prototype
{
    using System;
    
    public class Sandwich : SandwichPrototype
    {
        private string bread;
        private string meat;
        private string cheese;
        private string veggies;

        public Sandwich(string bread, string meat, string cheese, string veggies)
        {
            this.bread = bread;
            this.meat = meat;
            this.cheese = cheese;
            this.veggies = veggies;
        }

        public override SandwichPrototype Clone()
        {
            var ingredients = this.GetIngredients();

            Console.WriteLine("Cloning sandwich with ingredients: {0}", ingredients);

            return this.MemberwiseClone() as SandwichPrototype;
        }

        private string GetIngredients()
        {
            return $"{this.bread}, {this.meat}, {this.cheese}, {this.veggies}";
        }
    }
}
