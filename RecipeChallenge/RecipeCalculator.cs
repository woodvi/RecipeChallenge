using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeChallenge
{
    /// <summary>
    /// 
    /// Calculates the Sales Tax, Wellness Discount and Total cost of a recipe, based on the ingredients.
    /// 
    /// To use, 
    ///  call the constructor with a Recipe and an IngredientsDatabase, 
    ///  call Calculate() member function
    ///  retrieve desired data from Total, SubTotal, TaxableSubTotal, WellnessSubTotal, Tax & WellnessDiscount properties
    /// Example:
    ///  RecipeCalculator c = new RecipeCalculator(Recipe, IngredientsDatabase);
    ///  c.Calculate();
    ///  System.Console.WriteLine(c.Total);
    /// 
    /// Recipe must be a Recipe object, and its hashtable keywords must match the hashtable keywords in the IngredientsDatabase
    /// Calculator may be used repeatedly with different Recipe objects
    /// Rounding rules and tax rates may be optionally added to the constructor or the Calculate overloads
    /// 
    /// </summary>
    public class RecipeCalculator
    {
        public IngredientsDatabase IngredientsDatabase { get; set; }
        public Recipe Recipe { get; set; }

        public double Total { get; set; }
        public double SubTotal { get; set; }
        public double TaxableSubTotal { get; set; }
        public double WellnessSubTotal { get; set; }
        public double Tax { get; set; }
        public double WellnessDiscount { get; set; }

        public double SubtotalRounding { get; set; }
        public double TaxRate { get; set; }
        public double TaxRounding { get; set; }
        public double WellnessRate { get; set; }
        public double WellnessRounding { get; set; }

        double DEFAULT_SUBTOTALROUNDING = 0.01;
        double DEFAULT_TAXRATE = 0.086;
        double DEFAULT_TAXROUNDING = 0.07;
        double DEFAULT_WELLNESSRATE = -0.05;
        double DEFAULT_WELLNESSROUNDING = 0.01;


        /// <summary>
        /// Public constructor that takes a Recipe and an Ingredients Database, and calculates the totals
        /// </summary>
        /// <param name="Recipe">A recipe to calculate</param>
        /// <param name="IngredientsDatabase">an Ingredients Database</param>
        public RecipeCalculator(Recipe Recipe, IngredientsDatabase IngredientsDatabase)
        {
            this.Recipe = Recipe;
            this.IngredientsDatabase = IngredientsDatabase;

            Total = 0;
            SubTotal = 0;
            TaxableSubTotal = 0;
            WellnessSubTotal = 0;
            Tax = 0;
            WellnessDiscount = 0;

            this.SubtotalRounding = DEFAULT_SUBTOTALROUNDING;
            this.TaxRate = DEFAULT_TAXRATE;
            this.TaxRounding = DEFAULT_TAXROUNDING;
            this.WellnessRate = DEFAULT_WELLNESSRATE;
            this.WellnessRounding = DEFAULT_WELLNESSROUNDING;
        }

        /// <summary>
        /// Public constructor that takes a Recipe and an Ingredients Database, and calculates the totals
        /// </summary>
        /// <param name="Recipe">A recipe to calculate</param>
        /// <param name="IngredientsDatabase">an Ingredients Database</param>
        /// <param name="SubtotalRounding"></param>
        /// <param name="TaxRate"></param>
        /// <param name="TaxRounding"></param>
        /// <param name="WellnessRate"></param>
        /// <param name="WellnessRounding"></param>
        public RecipeCalculator(Recipe Recipe, IngredientsDatabase IngredientsDatabase, double SubtotalRounding, double TaxRate, double TaxRounding, double WellnessRate, double WellnessRounding)
        {
            this.Recipe = Recipe;
            this.IngredientsDatabase = IngredientsDatabase;

            Total = 0;
            SubTotal = 0;
            TaxableSubTotal = 0;
            WellnessSubTotal = 0;
            Tax = 0;
            WellnessDiscount = 0;

            this.SubtotalRounding = SubtotalRounding;
            this.TaxRate = TaxRate;
            this.TaxRounding = TaxRounding;
            this.WellnessRate = WellnessRate;
            this.WellnessRounding = WellnessRounding;

        }

        /// <summary>
        /// Calculates the taxes, discounts, and subtotals for the recipe
        /// 
        /// </summary>
        /// <returns>the overall total (other subtotals available through member properties)</returns>
        public double Calculate()
        {
            return (Calculate(this.Recipe, this.IngredientsDatabase, this.SubtotalRounding, this.TaxRate, this.TaxRounding, this.WellnessRate, this.WellnessRounding));
        }

        /// <summary>
        /// Calculates the taxes, discounts, and subtotals for the recipe using the existing ingredients database
        /// 
        /// </summary>
        /// <param name="Recipe">A recipe to calculate</param>
        /// <returns>the overall total (other subtotals available through member properties)</returns>
        public double Calculate(Recipe Recipe)
        {
            return (Calculate(Recipe, this.IngredientsDatabase, this.SubtotalRounding, this.TaxRate, this.TaxRounding, this.WellnessRate, this.WellnessRounding));
        }

        /// <summary>
        /// Calculates the taxes, discounts, and subtotals for the recipe
        /// 
        /// </summary>
        /// <param name="Recipe">A recipe to calculate</param>
        /// <param name="IngredientsDatabase">A database of ingredients data</param>
        /// <returns>the overall total (other subtotals available through member properties)</returns>
        public double Calculate(Recipe Recipe, IngredientsDatabase IngredientsDatabase)
        {
            return Calculate(Recipe, IngredientsDatabase, this.SubtotalRounding, this.TaxRate, this.TaxRounding, this.WellnessRate, this.WellnessRounding);
        }

        /// <summary>
        /// 
        /// Calculates the taxes, discounts, and subtotals for the recipe
        /// 
        /// </summary>
        /// <param name="Recipe">A recipe to calculate</param>
        /// <param name="IngredientsDatabase">A database of ingredients data</param>
        /// <param name="SubtotalRounding"></param>
        /// <param name="TaxRate"></param>
        /// <param name="TaxRounding"></param>
        /// <param name="WellnessRate"></param>
        /// <param name="WellnessRounding"></param>
        /// <returns>the overall total (other subtotals available through member properties)</returns>
        public double Calculate(Recipe Recipe, IngredientsDatabase IngredientsDatabase, double SubtotalRounding, double TaxRate, double TaxRounding, double WellnessRate, double WellnessRounding)
        {
            double LineItem;

            Total = 0;
            SubTotal = 0;
            TaxableSubTotal = 0;
            WellnessSubTotal = 0;

            foreach (String keyword in Recipe.Keys)
            {
                Recipe.Ingredient RecipeData = Recipe[keyword];
                if (IngredientsDatabase.ContainsKey(keyword))
                {
                    IngredientsDatabase.Inventory ShoppingData = IngredientsDatabase[keyword];

                    LineItem = RoundToFactor(
                        RecipeData.Quantity * ShoppingData.Price,
                        SubtotalRounding);

                    SubTotal += LineItem;
                    if (ShoppingData.IsTaxable())
                    {
                        TaxableSubTotal += LineItem;
                    }
                    if (ShoppingData.IsWellness())
                    {
                        WellnessSubTotal += LineItem;
                    }
                }
                else
                {
                    System.Console.Error.WriteLine(String.Format(
                        "Recipe '{0}' calls for '{1}'({2}), which is not found in the IngredientsDatabase",
                        Recipe.Title,
                        RecipeData.Description,
                        keyword));
                    // not doing anything counts the missing item as a cost of 0; marking the subtotal as NaN will invalidate the calculation
                    SubTotal = double.NaN;
                }
            }

            // calculate taxes, discount
            // TODO: clarify subtotal rounding, taxes-on-discount in spec
            // assumes that discount doesn't affect taxes; but since the spec calls for 7c rounding, it doesn't come up in any of the test cases
            WellnessDiscount = RoundToFactor(
                WellnessSubTotal * WellnessRate,
                WellnessRounding);

            Tax = RoundToFactor(
                TaxableSubTotal * TaxRate,
                TaxRounding);

            Total = SubTotal + Tax + WellnessDiscount;

            return (Total);
        }

        /// <summary>
        /// Rounds x up (away from zero) to the next highest multiple of round
        /// </summary>
        /// <param name="x">number to round</param>
        /// <param name="round">factor to round to</param>
        /// <returns></returns>
        static public double RoundToFactor(double x, double round)
        {
            if (round == 0)
            {
                return (x);
            }
            else
            {
                return (
                  Math.Ceiling(Math.Abs(x) / Math.Abs(round)) * Math.Abs(round) * Math.Sign(x)
                  );
            }
        }

    }
}
