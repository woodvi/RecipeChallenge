using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace RecipeChallenge
{
    /// <summary>
    /// implemented mainly as a Dictionary of Ingredients
    /// (also includes a title, and could include other properties)
    /// to get a recipe, call ReadRecipe on a TextReader with the Ingredients database,
    /// Example:
    ///  Recipe = Recipe.ReadRecipe( new System.IO.StreamReader( FileDescriptor ), IngredientsDatabase);
    /// </summary>
    public class Recipe : Dictionary<String, Recipe.Ingredient>
    {
        public const int MAX_TITLE_LEN = 80;    // arbitrary length that titles should be shorter than (because titles usually aren't very long)

        // our Recipe is implemented as a collection of ingredients (with a title; possible extension: include preparation text)
        public class Ingredient
        {
            /// <summary>
            /// Quantity of Ingredient in this Recipe
            /// </summary>
            public double Quantity { get; set; }
            /// <summary>
            /// Description of Ingredient in this Recipe
            /// </summary>
            public String Description { get; set; }

            public Ingredient() { }
            public Ingredient(double Quantity, String Description)
            {
                this.Quantity = Quantity;
                this.Description = Description;
            }
        }

        /// <summary>
        /// Title of Recipe
        /// </summary>
        public String Title { get; set; }

        public Recipe() { }
        public Recipe(String Title)
        {
            this.Title = Title;
        }

        public void Add(String key, double qty, String description)
        {
            this.Add(key, new Ingredient(qty, description));
        }

        public Boolean TryGetDescription(String key, out String description)
        {
            Ingredient ingredient;
            if (this.TryGetValue(key, out ingredient))
            {
                description = ingredient.Description;
                return true;
            }
            else
            {
                description = "";  // default value
                return false;
            }
        }

        public Boolean TryGetQuantity(String key, out double qty)
        {
            Ingredient ingredient;
            if (this.TryGetValue(key, out ingredient))
            {
                qty = ingredient.Quantity;
                return true;
            }
            else
            {
                qty = new double();  // default value
                return false;
            }
        }

        /// <summary>
        /// Reads in an Ingredient from a line of text
        /// Assumes that the text is in the format "[qty] [description]"
        /// eg: "2 1/4 bags of hugs" gets parsed into qty=2.25 description="bags of hugs"
        /// </summary>
        /// <param name="s">Reader (String or Stream) to read from</param>
        /// <param name="IngredientsCorpus">A collection of unique keywords that can be used as hashing keys</param>
        /// <returns>A new Recipe object represented by the text in s</returns>
        static public Recipe ReadRecipe(TextReader s, IngredientsDatabase IngredientsDatabase)
        {
            return( 
                ReadRecipe(s, IngredientsDatabase.Keys)
            );
        }

        /// <summary>
        /// Reads in an Ingredient from a line of text
        /// Assumes that the text is in the format "[qty] [description]"
        /// eg: "2 1/4 bags of hugs" gets parsed into qty=2.25 description="bags of hugs"
        /// </summary>
        /// <param name="s">Reader (String or Stream) to read from</param>
        /// <param name="IngredientsCorpus">A collection of unique keywords that can be used as hashing keys</param>
        /// <returns>A new Recipe object represented by the text in s</returns>
        static public Recipe ReadRecipe(TextReader s, ICollection<String> IngredientsCorpus)
        {
            Recipe r = new Recipe();
            String line;

            while (
                    null != (line = s.ReadLine())   // read a line, and only continue while there are more lines (TextReader does not have EOF, but using TextReader allows us to switch between reading strings and files)
                )
            {

                if (StringLooksLikeBlank(line))
                {
                    ;   // N-Op: discard blank line
                }
                else if (StringLooksLikeTitle(line))
                {
                    r.Title = line.Trim(); // store the title
                }
                else if (StringLooksLikeIngredient(line))
                {
                    String key = CalculateKey(line, IngredientsCorpus);
                    Ingredient Ingredient = ReadIngrediant(line);
                    if ((null != key) && (null != Ingredient))
                    {
                        r.Add(key, Ingredient);
                    }
                    else
                    {
                        // Error Handling

                        // do something sensible depending on the context; throwing an exception here might smack of "flow control"
                        // I'd like to discard the malformed line; and since this is a console application, lets send a warning to StdErr
                        if ((null == key) && (null == Ingredient))
                        {
                            System.Console.Error.WriteLine("Warning: Bad Key&Ingredient {0} in {1}", line, System.Environment.StackTrace);
                        }
                        else if (null == key)
                        {
                            System.Console.Error.WriteLine("Warning: Bad Key {0} in {1}", line, System.Environment.StackTrace);
                        }
                        else if (null == Ingredient)
                        {
                            System.Console.Error.WriteLine("Warning: Bad Ingredient {0} in {1}", line, System.Environment.StackTrace);
                        }
                    }
                }
                else
                {
                    ;   // N-Op: discard unrecognizeable lines
                }
            }

            return (r);
        }

        /// <summary>
        /// Reads in an Ingredient from a line of text
        /// Assumes that the text is in the format "[qty] [description]"
        /// eg: "2 1/4 bags of hugs" gets parsed into qty=2.25 description="bags of hugs"
        /// </summary>
        /// <param name="line">A line of text to parse into an ingredient</param>
        /// <returns>A new Ingredient object based on that text</returns>
        public static Ingredient ReadIngrediant(String line)
        {
            Regex rx = new Regex(@"^[\x33-\x40,\s,0-9,\/]+");
            if (rx.IsMatch(line))
            {
                // find and parse the quantity string
                Match match = rx.Match(line);
                String QuantityString = match.Value;
                double qty = ((double)((new FractionalNumber(QuantityString)).Result));
                // the remainder must be the description
                String Description = line.Substring(match.Index + match.Length).Trim().ToLower();
                // return the ingredient read
                return new Ingredient(qty, Description);
            }
            else
            {
                return new Ingredient(0, line);
            }
        }

        /// <summary>
        /// Finds which of the keword to use for as a hash key for storing ingredients (useful for rapid retrieval later)
        /// Picks the last keyword in the line of text that is in the known collection of keywords (IngredientsCorpus)
        /// </summary>
        /// <param name="line">A line of text in an Ingredient</param>
        /// <param name="IngredientsCorpus">A collection of unique keywords that can be used as hashing keys</param>
        /// <returns>The last String in IngredientsCorpus in that line</returns>
        public static String CalculateKey(String line, ICollection<String> IngredientsCorpus)
        {
            String key = null;
            foreach (String s in IngredientsCorpus)
            {
                if (line.Contains(s))
                {
                    key = s;
                }
            }
            return (key);
        }

        /// <summary>
        /// If there's nothing but whitespace in the line, then it's blank.
        /// </summary>
        /// <param name="line">a line of text to evaluate</param>
        /// <returns>true/false if the line looks blanke</returns>
        public static bool StringLooksLikeBlank(String line, bool beVerbose = false)
        {
            // shouldn't have anything but whitespace if it's a blank line
            Regex rx = new Regex(@"^[\s]*$");
            if (null == line)
            {
                // special convenience case: if it's null, treat it as a blank line (which usually gets ignored, therefore saving us any trouble of handling null pointer exceptions later)                  
                return true;
            }
            else if (beVerbose)
            {
                if (rx.IsMatch(line))
                {
                    Console.Error.WriteLine(String.Format("({0})", line));
                }
                else
                {
                    Console.Error.WriteLine(String.Format("~{0}~", line));
                }
            }
            return (rx.IsMatch(line));
        }

        /// <summary>
        // should start with a number (with any whitespace and most punctuation allowed) if it's an ingredient
        /// </summary>
        /// <param name="line">a line of text to evaluate</param>
        /// <returns>true/false if the line looks like an Ingredient</returns>
        public static bool StringLooksLikeIngredient(String line, bool beVerbose = false)
        {
            // should start with a number (with any whitespace and most punctuation allowed) if it's an ingredient
            Regex rx = new Regex(@"^[\x33-\x40,\s,\/]*[0-9]+.*[A-Z,a-z]+");
            if (beVerbose)
            {
                if (rx.IsMatch(line))
                {
                    Console.Error.WriteLine(String.Format("[{0}]", line));
                }
                else
                {
                    Console.Error.WriteLine(String.Format("!{0}!", line));
                }
            }
            return (rx.IsMatch(line));
        }

        /// <summary>
        /// should start with a capital letter (with any whitespace allowed) if it's a title
        /// </summary>
        /// <param name="line">a line of text to evaluate</param>
        /// <returns>true/false if the line looks like an Ingredient</returns>
        public static bool StringLooksLikeTitle(String line, bool beVerbose = false)
        {
            // should be short and start with a capital letter (with any whitespace allowed) if it's a title
            Regex rx = new Regex(@"^[\s]*[A-Z]+");
            if (beVerbose)
            {
                if ((line.Length <= MAX_TITLE_LEN) && rx.IsMatch(line))
                {
                    Console.Error.WriteLine(String.Format("<{0}>", line));
                }
                else
                {
                    Console.Error.WriteLine(String.Format("-{0}-", line));
                }
            }
            return ((line.Length <= MAX_TITLE_LEN) && (rx.IsMatch(line)));
        }

        /// <summary>
        /// should be long and include punctuation if it's instructions
        /// </summary>
        /// <param name="line">a line of text to evaluate</param>
        /// <returns>true/false if the line looks like instructions</returns>
        public static bool StringLooksLikeInstructions(String line, bool beVerbose = false)
        {
            // should be long and include letters and punctuation if it's instructions
            Regex rx = new Regex(@"[A-Z,a-z]+.*\.");
            if (beVerbose)
            {
                if ((line.Length > MAX_TITLE_LEN) && rx.IsMatch(line))
                {
                    Console.Error.WriteLine(String.Format("|{0}|", line));
                }
                else
                {
                    Console.Error.WriteLine(String.Format("?{0}?", line));
                }
            }
            return ((line.Length > MAX_TITLE_LEN) && (rx.IsMatch(line)));
        }


    }
}
