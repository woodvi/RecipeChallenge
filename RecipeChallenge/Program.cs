using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeChallenge
{
    class Program
    {
        // default file locations for Demo Mode
        private const String INGREDIENTSPATH = @"..\..\Ingredients.txt";
        private const String RECIPEPATH1 = @"..\..\Recipe1.txt";
        private const String RECIPEPATH2 = @"..\..\Recipe2.txt";
        private const String RECIPEPATH3 = @"..\..\Recipe3.txt";

        static void Main(string[] args)
        {
            string path = System.IO.Directory.GetCurrentDirectory();
            Console.WriteLine(path);


            if (0 == args.Length)
            {
                Help();
                Demo();
            }
            else
            {
                ICollection<String> keywords = null;
                IngredientsDatabase IngredientsDatabase = null;
                int count = 0;

                for(int idx = 0; idx < args.Length; idx++ )
                {
                    if (args[idx].StartsWith("?"))
                    {
                        // Display help
                        Help();
                    }
                    else if (args[idx].StartsWith("--"))
                    {
                        switch (args[idx].ToLower())
                        {
                            case "--help":
                                // Display help
                                Help();
                                break;
                            case "--demo":
                                // Display help
                                Demo();
                                break;
                            case "--db":
                            case "--database":
                            case "--ingredients":
                                if (0 != count)
                                {
                                    System.Console.WriteLine(String.Format("Calculated {0} recipes with a datbase of {1} items",
                                        count,
                                        (null == IngredientsDatabase) ? 0 : IngredientsDatabase.Count));
                                }

                                // Get Ingredients database
                                IngredientsDatabase = ReadDatabase(args, ref idx, ref keywords);
                                break;
                            default:
                                System.Console.Error.WriteLine(String.Format(
                                        "Ignoring unused option: {0}.", 
                                        args[idx] ));
                                break;
                        }
                    }
                    else
                    {
                        if ((null == keywords) || (0 == keywords.Count) || (null == IngredientsDatabase) || (0 == IngredientsDatabase.Count))
                        {
                            System.Console.Error.WriteLine(String.Format(
                                "Could not calculate recipe file {0} because no valid ingredients database has been loaded.",
                                args[idx]));
                        }
                        else
                        {
                            // Calculate a Recipe file
                            Recipe Recipe = ReadRecipe(args, idx, keywords);
                            CalculateAndDisplayRecipe(Recipe, IngredientsDatabase);
                            count++;
                        }
                    }                        
                }
                System.Console.WriteLine(String.Format("Calculated {0} recipes with a datbase of {1} items", 
                    count, 
                    (null==IngredientsDatabase)?0:IngredientsDatabase.Count));
            }
        }

        /// <summary>
        /// opens a file and reads the recipe; calculates the cost
        /// (mostly exception handling)
        /// </summary>
        /// <param name="args"></param>
        /// <param name="keywords"></param>
        /// <param name="IngredientsDatabase"></param>
        /// <param name="idx"></param>
        /// <returns></returns>
        private static Recipe ReadRecipe(string[] args, int idx, ICollection<String> keywords)
        {
            Recipe Recipe = null;
            System.IO.StreamReader RecipeStream = null;
            try
            {
                // Do Recipe File
                RecipeStream = new System.IO.StreamReader(args[idx]);
                Recipe = Recipe.ReadRecipe(RecipeStream, keywords);
            }
            // FIXME: catch-all exception handling for code readability
            catch (System.Exception /*e*/ ) 
            {
                System.Console.Error.WriteLine(String.Format(
                    "Could not open ingredients file: {0}.",
                    args[idx]));
            }
            finally
            {
                if (RecipeStream != null)
                {
                    RecipeStream.Close();
                }
            }
            return( Recipe );
        }

        /// <summary>
        /// opens a file and reads the ingredients
        /// (mostly exception handling)
        /// </summary>
        /// <param name="args"></param>
        /// <param name="keywords"></param>
        /// <param name="idx"></param>
        /// <returns></returns>
        private static IngredientsDatabase ReadDatabase(string[] args, ref int idx, ref ICollection<String> keywords)
        {
            IngredientsDatabase IngredientsDatabase = null;

            System.IO.StreamReader IngredientsStream = null;
            try
            {
                // Do Ingredients File
                IngredientsStream = new System.IO.StreamReader(args[++idx]);
                IngredientsDatabase = IngredientsDatabase.ReadIngredientsDatabase(IngredientsStream, out keywords);
            }
            // FIXME: catch-all exception handling for code readability
            catch (System.Exception /*e*/ ) 
            {
                System.Console.Error.WriteLine(String.Format(
                    "Could not open ingredients file: {0}.",
                    args[idx]));
            }
            finally
            {
                if (IngredientsStream != null)
                {
                    IngredientsStream.Close();
                }
            }
            return( IngredientsDatabase );
        }

        private static void Help(){
            System.Console.WriteLine("RecipeChallenge");
            System.Console.WriteLine("v0.01");
            System.Console.WriteLine("Victor I. Wood");
            System.Console.WriteLine("2014-Jan-20");
            System.Console.WriteLine();

            System.Console.WriteLine("Usage:");
            System.Console.WriteLine("RecipeChallenge --database Ingredients.txt Recipe1.txt Recipe2.txt Recipe3.txt");
            System.Console.WriteLine();
            System.Console.WriteLine(
                                "\t--help\n" +
                                "\t\tthis message\n");
            System.Console.WriteLine(
                                "\t--demo\n" +
                                "\t\truns a demo using default files\n");
            System.Console.WriteLine(
                                "\t--db <dbfile>, --database <dbfile>, --ingredients <dbfile>\n" +
                                "\t\treads dbfile as an items database, must be before any recipe files.  Will be used for all following recipes.  No recipe can be calculated without a database.");
        }

        private static void Demo()
        {
            System.IO.StreamReader RecipeStream;
            ICollection<String> keywords;

            System.Console.WriteLine("Demo Mode");
            System.Console.WriteLine();

            // Get Ingredients database
            System.IO.StreamReader IngredientsStream = new System.IO.StreamReader(INGREDIENTSPATH);
            IngredientsDatabase IngredientsDatabase = IngredientsDatabase.ReadIngredientsDatabase(IngredientsStream, out keywords);

            // Do Recipe 1
            RecipeStream = new System.IO.StreamReader(RECIPEPATH1);
            Recipe Recipe1 = Recipe.ReadRecipe(RecipeStream, keywords);
            CalculateAndDisplayRecipe(Recipe1, IngredientsDatabase);
            RecipeStream.Close();

            // Do Recipe 2
            RecipeStream = new System.IO.StreamReader(RECIPEPATH2);
            Recipe Recipe2 = Recipe.ReadRecipe(RecipeStream, keywords);
            CalculateAndDisplayRecipe(Recipe2, IngredientsDatabase);
            RecipeStream.Close();

            // Do Recipe 3
            RecipeStream = new System.IO.StreamReader(RECIPEPATH3);
            Recipe Recipe3 = Recipe.ReadRecipe(RecipeStream, keywords);
            CalculateAndDisplayRecipe(Recipe3, IngredientsDatabase);
            RecipeStream.Close();
        }

        private static void CalculateAndDisplayRecipe(Recipe Recipe, IngredientsDatabase IngredientsDatabase)
        {
            // Calculate
            RecipeCalculator Calculator = new RecipeCalculator(Recipe, IngredientsDatabase);
            Calculator.Calculate(Recipe);
            // Display
            System.Console.WriteLine(Recipe.Title);
            System.Console.WriteLine(String.Format("Tax = {0:C}", Calculator.Tax));
            System.Console.WriteLine(String.Format("Discount = {0:C}", Calculator.WellnessDiscount));
            System.Console.WriteLine(String.Format("Total = {0:C}", Calculator.Total));
            System.Console.WriteLine();
        }
    }
}
