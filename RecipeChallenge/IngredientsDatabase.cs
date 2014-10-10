using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;


namespace RecipeChallenge
{
    /// <summary>
    /// a Dictionary of Inventory items that can be used as ingredients in recipes,
    /// each item contains a category, description and price.
    /// Dictionary uses keywords inside the inventory item description to hash, to allow inexact strings to match up
    /// To use, call the static IngredientsDatabase.ReadIngredientsDatabase function on a TextReader
    /// Example:
    ///  IngredientsDatabase = IngredientsDatabase.ReadIngredientsDatabase( new System.IO.StreamReader( FileDescriptor ) );
    /// </summary>
    public class IngredientsDatabase : System.Collections.Generic.Dictionary<String, IngredientsDatabase.Inventory>
    {

        // our IngredientsDatabase is implemented as a collection of inventory items
        public class Inventory
        {
            public static readonly String[] ListOfWellnessItemNames = new String[] { "organic" };
            public static readonly String[] ListOfNonTaxableCategoryNames = new String[] { "produce" };

            public String Category { get; set; }
            public String Description { get; set; }
            public double Price { get; set; }

            public Inventory() { }
            public Inventory(String Category, String Description, double Price)
            {
                this.Category = Category;
                this.Description = Description;
                this.Price = Price;
            }

            /// <summary>
            /// taxable items are all except the ones tagged in the category with keywords in the ListOfNonTaxableCategoryNames
            /// 
            /// </summary>
            /// <returns>true/false if the item is taxable</returns>
            public Boolean IsTaxable()
            {
                return IsTaxable(this.Category, ListOfNonTaxableCategoryNames);
            }

            /// <summary>
            /// taxable items are all except the ones tagged in the category with keywords in the ListOfNonTaxableCategoryNames
            /// 
            /// </summary>
            /// <param name="ListOfNonTaxableCategoryNames"></param>
            /// <returns>true/false if the item is taxable</returns>
            public Boolean IsTaxable(String[] ListOfNonTaxableCategoryNames)
            {
                return IsTaxable(this.Category, ListOfNonTaxableCategoryNames);
            }

            /// <summary>
            /// taxable items are all except the ones tagged in the category with keywords in the ListOfNonTaxableCategoryNames
            /// 
            /// </summary>
            /// <param name="Category">Category String to scan for Nontaxable Exemption keywords</param>
            /// <param name="ListOfNonTaxableCategoryNames">Nontaxable Exemption keywords to scan for</param>
            /// <returns>true/false if the item is taxable</returns>
            static public Boolean IsTaxable(String Category, ICollection<String> ListOfNonTaxableCategoryNames)
            {
                // taxable items are all except the ones tagged in the category with keywords in the ListOfNonTaxableCategoryNames
                Boolean IsTaxable = true;

                if ((null == ListOfNonTaxableCategoryNames) || (0 == ListOfNonTaxableCategoryNames.Count))
                {
                    // if there's no list of taxable items, then we probably have a problem; and taxes are the sort of thing that can get you into trouble
                    System.Console.Error.WriteLine("Warning: no taxable category definition in {0}", System.Environment.StackTrace);
                    return IsTaxable;   // when in doubt: it's taxable
                }
                else if ((null == Category) || (0 == Category.Length))
                {
                    return IsTaxable;   // when in doubt: it's taxable
                }
                else foreach (String s in ListOfNonTaxableCategoryNames)
                {
                    IsTaxable = IsTaxable && !(Category.ToLower().Contains(s.ToLower().Trim()));
                }
                return IsTaxable;
            }

            /// <summary>
            /// wellness items are only the ones tagged in the description with keywords in the ListOfWellnessItemNames
            /// 
            /// </summary>
            /// <returns>true/false if the item qualifies for the Wellness Discount</returns>
            public Boolean IsWellness()
            {
                return Inventory.IsWellness(this.Description, ListOfWellnessItemNames);
            }

            /// <summary>
            /// wellness items are only the ones tagged in the description with keywords in the ListOfWellnessItemNames
            /// 
            /// </summary>
            /// <param name="ListOfWellnessItemNames"></param>
            /// <returns>true/false if the item qualifies for the Wellness Discount</returns>
            public Boolean IsWellness(String[] ListOfWellnessItemNames)
            {
                return Inventory.IsWellness(this.Description, ListOfWellnessItemNames);
            }

            /// <summary>
            /// wellness items are only the ones tagged in the description with keywords in the ListOfWellnessItemNames
            /// 
            /// </summary>
            /// <param name="Description">Description String to scan for keywords</param>
            /// <param name="ListOfWellnessItemNames">Wellness item keywords to scan for</param>
            /// <returns>true/false if the item qualifies for the Wellness Discount</returns>
            static public Boolean IsWellness(String Description, ICollection<String> ListOfWellnessItemNames)
            {
                // wellness items are only the ones tagged in the description with keywords in the ListOfWellnessItemNames
                Boolean IsWellness = false;

                if ((null == ListOfWellnessItemNames) || (0 == ListOfWellnessItemNames.Count))
                {
                    // significantly less serious than messing up on your taxes, but if we get here, then we should still probably say something
                    System.Console.Error.WriteLine("Warning: no wellness items definition in {0}", System.Environment.StackTrace);
                    return IsWellness;   // when in doubt: it's not a wellness item
                }
                else if ((null == Description) || (0 == Description.Length))
                {
                    return IsWellness;   // when in doubt: it's not a wellness item
                }
                else foreach (String s in ListOfWellnessItemNames)
                {
                    IsWellness = IsWellness || (Description.ToLower().Contains(s.ToLower().Trim()));
                }
                return IsWellness;
            }



        }

        public class IngredientsDatabaseReader 
        {
            public const int MAX_CATEGORY_LEN = 80;    // arbitrary length that titles should be shorter than (because titles usually aren't very long)

            TextReader Reader;
            String CurrentCategory = "Uncategoried";

            public IngredientsDatabaseReader( TextReader r )
            {
                Reader = r;
            }
            public Inventory ReadLine(int maxDepth = 32)
            {
                String line;

                if (maxDepth > 0)
                {
                    line = Reader.ReadLine();
                    if (null == line)
                    {
                        // EOF
                        return null;
                    }
                    else if (StringLooksLikeBlank(line))
                    {
                        ;   // N-Op: discard blank line
                        return (ReadLine(maxDepth - 1));    // move on to the next line
                    }
                    else if (StringLooksLikeCategory(line))
                    {
                        CurrentCategory = line.Trim();      // change the current category
                        return (ReadLine(maxDepth - 1));    // move on to the next line
                    }
                    else if (StringLooksLikeIngredient(line))
                    {
                        // return result
                        return (ReadInventory(line, CurrentCategory));
                    } else {
                        ;   // N-Op: discard unrecognizeable lines
                        return (ReadLine(maxDepth - 1));    // move on to the next line
                    }
                }
                else
                {
                    // recursion safety trigger: exhausted maxDepth; note that this is indistinguishable from EOF
                    return null;
                }
            }

            /// <summary>
            /// Reads in an Ingredient from a line of text
            /// Assumes that the text is in the format "[qty] [description]"
            /// eg: "2 1/4 bags of hugs = $2.25" gets parsed into description="bags of hugs", price = 1.00 (because 2 1/4 = $2.25, so they must be $1 each)
            /// </summary>
            /// <param name="line">A line of text to parse into an ingredient</param>
            /// <returns>A new Ingredient object based on that text</returns>
            public static Inventory ReadInventory(String line, String CurrentCategory)
            {
                Regex rx = new Regex(@"^[\x33-\x40,\s,0-9,\/]+");
                double Price = double.NaN;
                String DescriptionString;
                 String PriceString;

                 if ((null == line) || (String.IsNullOrEmpty(line)))
                 {
                     return null;
                 }
                 else
                 {

                     String[] tokens = line.Split('=');

                     DescriptionString = (tokens.Length >= 1) ? tokens[0] : "";
                     PriceString = (tokens.Length >= 2) ? tokens[1] : "";
                     Price = (PriceString.Length > 0) ? double.Parse(PriceString, NumberStyles.Currency) : 0;

                     if (rx.IsMatch(DescriptionString))
                     {
                         // find and parse the quantity string
                         Match match = rx.Match(DescriptionString);
                         String QuantityString = match.Value.Trim();
                         double qty;
                         try
                         {
                             qty = ((double)((new FractionalNumber(QuantityString)).Result));
                         }
                         catch (ArgumentNullException /*e*/) // FractionalNumer.Parse throws ArgumentNullException if the string is blank
                         {
                             qty = 0;
                         }

                         // the remainder must be the description
                         String Description = DescriptionString.Substring(match.Index + match.Length).Trim().ToLower();
                         // return the ingredient read
                         return new Inventory(TransformCategory(CurrentCategory), Description, (qty > 0) ? Price / qty : Price);
                     }
                     else
                     {
                         return new Inventory(TransformCategory(CurrentCategory), line, 0);
                     }
                 }
            }

            /// <summary>
            /// trims whitespace and converts to lower case, to make it easier to match up strings
            /// </summary>
            /// <param name="str">string to transform</param>
            /// <returns>a new transformed string</returns>
            static private String TransformCategory(String str)
            {
                return (
                    (null != str) ? str.Trim().ToLower() : ""
                    );
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
                if (beVerbose) {
                    if( rx.IsMatch(line))
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
            public static bool StringLooksLikeCategory(String line, bool beVerbose = false)
            {
                // should be short and start with a letter (with any whitespace allowed) if it's a title
                Regex rx = new Regex(@"^[\s]*[A-Z,a-z]+");
                if (beVerbose)
                {
                    if ((line.Length <= MAX_CATEGORY_LEN) && rx.IsMatch(line))
                    {
                        Console.Error.WriteLine(String.Format("<{0}>", line));
                    }
                    else
                    {
                        Console.Error.WriteLine(String.Format("-{0}-", line));
                    }
                }
                return ((line.Length <= MAX_CATEGORY_LEN) && (rx.IsMatch(line)));
            }


        }

        /// <summary>
        /// Generates a database of Ingredints from a text stream
        /// </summary>
        /// <param name="r">TextReader containing data</param>
        /// <returns>a new IngredientsDatabase object</returns>
        static public IngredientsDatabase ReadIngredientsDatabase(TextReader r)
        {
            ICollection<String> IngredientsCorpus = new System.Collections.Generic.HashSet<String>();
            return ReadIngredientsDatabase( r, out IngredientsCorpus );
        }

        /// <summary>
        /// Generates a database of Ingredints from a text stream
        /// </summary>
        /// <param name="r">TextReader containing data</param>
        /// <param name="IngredientsCorpus">an output parameter with unique keywords which will be used for the hash table keys</param>
        /// <returns>a new IngredientsDatabase object</returns>
        static public IngredientsDatabase ReadIngredientsDatabase(TextReader r, out ICollection<String> IngredientsCorpus)
        {
            IngredientsDatabase db = new IngredientsDatabase();
            IngredientsDatabaseReader Reader = new IngredientsDatabaseReader(r);
            IngredientsCorpus = new System.Collections.Generic.HashSet<String>();

            Inventory Inventory;
            while (
                    null != (Inventory = Reader.ReadLine())   // read a line, and only continue while there are more lines (TextReader does not have EOF, but using TextReader allows us to switch between reading strings and files)
                )
            {
                String keyword = CalculateKeyword(Inventory);
                db.Add(keyword, Inventory);
                IngredientsCorpus.Add(keyword);
            }

            return db;
        }

        /// <summary>
        /// picks a keyword to use for the hash table (and therefore the recipe join)
        /// </summary>
        /// <param name="Inventory">Ingredient to find a hash keyword for</param>
        /// <returns>a String to use as the hash key</returns>
        private static string CalculateKeyword(Inventory Inventory)
        {
            Regex rx = new Regex(@"\s");
            String[] tokens = rx.Split(Inventory.Description);
            String keyword = tokens[tokens.Length -1];      // in English language, the actual modified noun tends to be at the end of the phrase, eg "the quick brown fox"; there's lots of ways to come up with this join logic, but I felt that this was fairly intuitive
            return keyword;
        }

    }
}
