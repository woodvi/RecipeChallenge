using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RecipeChallenge;
using System.Collections.Generic;
using System.IO;

namespace UnitTestProject
{
    [TestClass]
    public class IngredientsDatabaseTest
    {

        [TestMethod]
        public void IsTaxableStatic()
        {
            String[] ListOfNonTaxableCategoryNames = new String[] { "produce", "x_nontaxable" };

            Assert.IsTrue(
                RecipeChallenge.IngredientsDatabase.Inventory.IsTaxable("PANTRY", ListOfNonTaxableCategoryNames)
                );

            Assert.IsTrue(
                RecipeChallenge.IngredientsDatabase.Inventory.IsTaxable("Meat/Poultry", ListOfNonTaxableCategoryNames)
                );

            Assert.IsTrue(
                RecipeChallenge.IngredientsDatabase.Inventory.IsTaxable("organic blueberries", ListOfNonTaxableCategoryNames)
                );

            Assert.IsFalse(
                RecipeChallenge.IngredientsDatabase.Inventory.IsTaxable("x_megavitamin produce", ListOfNonTaxableCategoryNames)
                );

            Assert.IsTrue(
                RecipeChallenge.IngredientsDatabase.Inventory.IsTaxable("taxable unicorns", ListOfNonTaxableCategoryNames)
                );

            Assert.IsFalse(
                RecipeChallenge.IngredientsDatabase.Inventory.IsTaxable("x_nontaxable unicorns", ListOfNonTaxableCategoryNames)
                );

            Assert.IsTrue(
                RecipeChallenge.IngredientsDatabase.Inventory.IsTaxable("a pinch of melancholy", ListOfNonTaxableCategoryNames)
                );

            Assert.IsTrue(
                RecipeChallenge.IngredientsDatabase.Inventory.IsTaxable(" ", ListOfNonTaxableCategoryNames)
                );

            Assert.IsTrue(
                RecipeChallenge.IngredientsDatabase.Inventory.IsTaxable("", ListOfNonTaxableCategoryNames)
                );

            Assert.IsTrue(
                RecipeChallenge.IngredientsDatabase.Inventory.IsTaxable(null, ListOfNonTaxableCategoryNames)
                );

            Assert.IsTrue(
                RecipeChallenge.IngredientsDatabase.Inventory.IsTaxable("Undefined Taxable Categories", null)
                );

        }

        [TestMethod]
        public void IsTaxableMember()
        {
            String[] ListOfNonTaxableCategoryNames = new String[] { "produce", "x_nontaxable" };
            IngredientsDatabase.Inventory i0 = new IngredientsDatabase.Inventory("Pantry", "Hugs", 0.99);

            i0.Category = "PANTRY";
            Assert.IsTrue(i0.IsTaxable());

            i0.Category = "Meat/Poultry";
            Assert.IsTrue(i0.IsTaxable());

            i0.Category = "organic blueberries";
            Assert.IsTrue(i0.IsTaxable());

            i0.Category = "x_megavitamin produce";
            Assert.IsFalse(i0.IsTaxable());

            i0.Category = "taxable unicorns";
            Assert.IsTrue(i0.IsTaxable());

            // "x_nontaxable" is a test category only, so will be False only if we override the ListOfNonTaxableCategoryNames
            i0.Category = "x_nontaxable unicorns";
            Assert.IsTrue(i0.IsTaxable());
            i0.Category = "x_nontaxable unicorns";
            Assert.IsFalse(i0.IsTaxable(ListOfNonTaxableCategoryNames));

            i0.Category = "a pinch of melancholy";
            Assert.IsTrue(i0.IsTaxable());

            i0.Category = " ";
            Assert.IsTrue(i0.IsTaxable());

            i0.Category = "";
            Assert.IsTrue(i0.IsTaxable());

            i0.Category = "null";
            Assert.IsTrue(i0.IsTaxable());

        }

        [TestMethod]
        public void IsWellnessStatic()
        {
            String[] ListOfWellnessItemNames = new String[] { "organic", "x_megavitamin" };

            Assert.IsFalse(
                RecipeChallenge.IngredientsDatabase.Inventory.IsWellness("PANTRY", ListOfWellnessItemNames)
                );

            Assert.IsFalse(
                RecipeChallenge.IngredientsDatabase.Inventory.IsWellness("Meat/Poultry", ListOfWellnessItemNames)
                );

            Assert.IsTrue(
                RecipeChallenge.IngredientsDatabase.Inventory.IsWellness("organic blueberries", ListOfWellnessItemNames)
                );

            Assert.IsTrue(
                RecipeChallenge.IngredientsDatabase.Inventory.IsWellness("x_megavitamin produce", ListOfWellnessItemNames)
                );

            Assert.IsFalse(
                RecipeChallenge.IngredientsDatabase.Inventory.IsWellness("taxable unicorns", ListOfWellnessItemNames)
                );

            Assert.IsFalse(
                RecipeChallenge.IngredientsDatabase.Inventory.IsWellness("x_nontaxable unicorns", ListOfWellnessItemNames)
                );

            Assert.IsFalse(
                RecipeChallenge.IngredientsDatabase.Inventory.IsWellness("a pinch of melancholy", ListOfWellnessItemNames)
                );

            Assert.IsFalse(
                RecipeChallenge.IngredientsDatabase.Inventory.IsWellness(" ", ListOfWellnessItemNames)
                );

            Assert.IsFalse(
                RecipeChallenge.IngredientsDatabase.Inventory.IsWellness("", ListOfWellnessItemNames)
                );

            Assert.IsFalse(
                RecipeChallenge.IngredientsDatabase.Inventory.IsWellness(null, ListOfWellnessItemNames)
                );

            Assert.IsFalse(
                RecipeChallenge.IngredientsDatabase.Inventory.IsWellness("Undefined Wellness Keywords", null)
                );
        }

        [TestMethod]
        public void IsWellnessMember()
        {
            String[] ListOfWellnessItemNames = new String[] { "organic", "x_megavitamin" };
            IngredientsDatabase.Inventory i0 = new IngredientsDatabase.Inventory("Pantry", "Hugs", 0.99);

            i0.Description = "PANTRY";
            Assert.IsFalse(i0.IsWellness());

            i0.Description = "Meat/Poultry";
            Assert.IsFalse(i0.IsWellness());

            i0.Description = "organic blueberries";
            Assert.IsTrue(i0.IsWellness());

            // x_megavitamin is a test category only, so will be True only if we override the ListOfWellnessItemNames
            i0.Description = "x_megavitamin produce";
            Assert.IsFalse(i0.IsWellness());
            i0.Description = "x_megavitamin produce";
            Assert.IsTrue(i0.IsWellness(ListOfWellnessItemNames));

            i0.Description = "taxable unicorns";
            Assert.IsFalse(i0.IsWellness());

            i0.Description = "x_nontaxable unicorns";
            Assert.IsFalse(i0.IsWellness());

            i0.Description = "a pinch of melancholy";
            Assert.IsFalse(i0.IsWellness());

            i0.Description = " ";
            Assert.IsFalse(i0.IsWellness());

            i0.Description = "";
            Assert.IsFalse(i0.IsWellness());

            i0.Description = null;
            Assert.IsFalse(i0.IsWellness());

        }

        [TestMethod]
        public void ReadIngrediant()
        {

            IngredientsDatabase.Inventory Ingredient;
            Ingredient = IngredientsDatabase.IngredientsDatabaseReader.ReadInventory(" 1 cup of corn = $0.87 ", "Produce");
            Assert.IsNotNull(Ingredient);
            Assert.AreEqual("produce", Ingredient.Category);
            Assert.AreEqual("cup of corn", Ingredient.Description);
            Assert.AreEqual(0.87, Ingredient.Price);

            Ingredient = IngredientsDatabase.IngredientsDatabaseReader.ReadInventory(" 4 slice of bacon = $0.96 ", "Meat/poultry");
            Assert.IsNotNull(Ingredient);
            Assert.AreEqual("meat/poultry", Ingredient.Category);
            Assert.AreEqual("slice of bacon", Ingredient.Description);
            Assert.AreEqual(0.24, Ingredient.Price);

            Ingredient = IngredientsDatabase.IngredientsDatabaseReader.ReadInventory(" 3 ounce of pasta = $0.93 ", "Pantry");
            Assert.IsNotNull(Ingredient);
            Assert.AreEqual("pantry", Ingredient.Category);
            Assert.AreEqual("ounce of pasta", Ingredient.Description);
            Assert.AreEqual(0.31, Ingredient.Price);

            Ingredient = IngredientsDatabase.IngredientsDatabaseReader.ReadInventory(" 2 turtle doves ", "Misc");    // no price
            Assert.IsNotNull(Ingredient);
            Assert.AreEqual("misc", Ingredient.Category);
            Assert.AreEqual("turtle doves", Ingredient.Description);
            Assert.AreEqual(0, Ingredient.Price);

            Ingredient = IngredientsDatabase.IngredientsDatabaseReader.ReadInventory(" The Ultimate Answer = 42", "Misc");    // no qty, no currency symbole
            Assert.IsNotNull(Ingredient);
            Assert.AreEqual("misc", Ingredient.Category);
            Assert.AreEqual("the ultimate answer", Ingredient.Description);
            Assert.AreEqual(42, Ingredient.Price);

            Ingredient = IngredientsDatabase.IngredientsDatabaseReader.ReadInventory(" partridge in a pear tree", "Misc");    // nonsense string
            Assert.IsNotNull(Ingredient);
            Assert.AreEqual("misc", Ingredient.Category);
            Assert.AreEqual("partridge in a pear tree", Ingredient.Description);
            Assert.AreEqual(0, Ingredient.Price);

            Ingredient = IngredientsDatabase.IngredientsDatabaseReader.ReadInventory("", "Misc");    // blank string
            Assert.IsNull(Ingredient);

            Ingredient = IngredientsDatabase.IngredientsDatabaseReader.ReadInventory(null, "Misc");    // null string
            Assert.IsNull(Ingredient);

            Ingredient = IngredientsDatabase.IngredientsDatabaseReader.ReadInventory(" 9 geese a-laying = $0.99", null);    // null category
            Assert.IsNotNull(Ingredient);
            Assert.AreEqual("", Ingredient.Category);
            Assert.AreEqual("geese a-laying", Ingredient.Description);
            Assert.AreEqual(0.11, Ingredient.Price);

            Ingredient = IngredientsDatabase.IngredientsDatabaseReader.ReadInventory(" 9 geese a-laying = $0.99", "");    // blank category
            Assert.IsNotNull(Ingredient);
            Assert.AreEqual("", Ingredient.Category);
            Assert.AreEqual("geese a-laying", Ingredient.Description);
            Assert.AreEqual(0.11, Ingredient.Price);

        }

        [TestMethod]
        public void ReadLine()
        {

            String SampleText =
                "Ingredients " + "\n" +
                "" + "\n" +
                "Produce " + "\n" +
                " 1 clove of organic garlic = $0.67 " + "\n" +
                " 1 Lemon = $2.03 " + "\n" +
                " 1 cup of corn = $0.87 " + "\n" +
                "" + "\n" +
                "Meat/poultry " + "\n" +
                " 1 chicken breast = $2.19 " + "\n" +
                " 1 slice of bacon = $0.24 " + "\n" +
                "" + "\n" +
                "Pantry " + "\n" +
                " 1 ounce of pasta = $0.31 " + "\n" +
                " 1 cup of organic olive oil = $1.92 " + "\n" +
                " 1 cup of vinegar = $1.26 " + "\n" +
                " 1 teaspoon of salt = $0.16 " + "\n" +
                " 1 teaspoon of pepper = $0.17 " + "\n";


            TextReader r0 = new StringReader(SampleText);
            IngredientsDatabase.IngredientsDatabaseReader r = new IngredientsDatabase.IngredientsDatabaseReader(r0);

            IngredientsDatabase.Inventory Ingredient;

            Ingredient = r.ReadLine();
            Assert.IsNotNull(Ingredient);
            Assert.AreEqual("produce", Ingredient.Category);
            Assert.AreEqual("clove of organic garlic", Ingredient.Description);
            Assert.AreEqual(0.67, Ingredient.Price);

            Ingredient = r.ReadLine();
            Assert.IsNotNull(Ingredient);
            Assert.AreEqual("produce", Ingredient.Category);
            Assert.AreEqual("lemon", Ingredient.Description);
            Assert.AreEqual(2.03, Ingredient.Price);

            Ingredient = r.ReadLine();
            Assert.IsNotNull(Ingredient);
            Assert.AreEqual("produce", Ingredient.Category);
            Assert.AreEqual("cup of corn", Ingredient.Description);
            Assert.AreEqual(0.87, Ingredient.Price);

            Ingredient = r.ReadLine();
            Assert.IsNotNull(Ingredient);
            Assert.AreEqual("meat/poultry", Ingredient.Category);
            Assert.AreEqual("chicken breast", Ingredient.Description);
            Assert.AreEqual(2.19, Ingredient.Price);

            Ingredient = r.ReadLine();
            Assert.IsNotNull(Ingredient);
            Assert.AreEqual("meat/poultry", Ingredient.Category);
            Assert.AreEqual("slice of bacon", Ingredient.Description);
            Assert.AreEqual(0.24, Ingredient.Price);

            Ingredient = r.ReadLine();
            Assert.IsNotNull(Ingredient);
            Assert.AreEqual("pantry", Ingredient.Category);
            Assert.AreEqual("ounce of pasta", Ingredient.Description);
            Assert.AreEqual(0.31, Ingredient.Price);

            Ingredient = r.ReadLine();
            Assert.IsNotNull(Ingredient);
            Assert.AreEqual("pantry", Ingredient.Category);
            Assert.AreEqual("cup of organic olive oil", Ingredient.Description);
            Assert.AreEqual(1.92, Ingredient.Price);

            Ingredient = r.ReadLine();
            Assert.IsNotNull(Ingredient);
            Assert.AreEqual("pantry", Ingredient.Category);
            Assert.AreEqual("cup of vinegar", Ingredient.Description);
            Assert.AreEqual(1.26, Ingredient.Price);

            Ingredient = r.ReadLine();
            Assert.IsNotNull(Ingredient);
            Assert.AreEqual("pantry", Ingredient.Category);
            Assert.AreEqual("teaspoon of salt", Ingredient.Description);
            Assert.AreEqual(0.16, Ingredient.Price);

            Ingredient = r.ReadLine();
            Assert.IsNotNull(Ingredient);
            Assert.AreEqual("pantry", Ingredient.Category);
            Assert.AreEqual("teaspoon of pepper", Ingredient.Description);
            Assert.AreEqual(0.17, Ingredient.Price);

            Ingredient = r.ReadLine();
            Assert.AreEqual(null, Ingredient);

        }

        [TestMethod]
        public void ReadIngredientsDatabase()
        {

            String SampleText =
                "Ingredients " + "\n" +
                "" + "\n" +
                "Produce " + "\n" +
                " 1 clove of organic garlic = $0.67 " + "\n" +
                " 1 Lemon = $2.03 " + "\n" +
                " 1 cup of corn = $0.87 " + "\n" +
                "" + "\n" +
                "Meat/poultry " + "\n" +
                " 1 chicken breast = $2.19 " + "\n" +
                " 1 slice of bacon = $0.24 " + "\n" +
                "" + "\n" +
                "Pantry " + "\n" +
                " 1 ounce of pasta = $0.31 " + "\n" +
                " 1 cup of organic olive oil = $1.92 " + "\n" +
                " 1 cup of vinegar = $1.26 " + "\n" +
                " 1 teaspoon of salt = $0.16 " + "\n" +
                " 1 teaspoon of pepper = $0.17 " + "\n";


            TextReader Reader = new StringReader(SampleText);

            ICollection<String> IngredientsCorpus;
            IngredientsDatabase db = IngredientsDatabase.ReadIngredientsDatabase(Reader, out IngredientsCorpus);


            // check the corpus
            Assert.IsNotNull(IngredientsCorpus);

            Assert.IsTrue(IngredientsCorpus.Contains("garlic"));
            Assert.IsTrue(IngredientsCorpus.Contains("lemon"));
            Assert.IsTrue(IngredientsCorpus.Contains("corn"));
            Assert.IsTrue(IngredientsCorpus.Contains("breast"));
            Assert.IsTrue(IngredientsCorpus.Contains("bacon"));
            Assert.IsTrue(IngredientsCorpus.Contains("pasta"));
            Assert.IsTrue(IngredientsCorpus.Contains("oil"));
            Assert.IsTrue(IngredientsCorpus.Contains("vinegar"));
            Assert.IsTrue(IngredientsCorpus.Contains("salt"));
            Assert.IsTrue(IngredientsCorpus.Contains("pepper"));

            Assert.IsFalse(IngredientsCorpus.Contains("unicorns"));
            Assert.IsFalse(IngredientsCorpus.Contains("rainbows"));
            Assert.IsFalse(IngredientsCorpus.Contains("antimatter"));

            Assert.IsFalse(IngredientsCorpus.Contains(""));

            // check the database
            Assert.IsNotNull(db);

            Assert.IsTrue(db.ContainsKey("garlic"));
            Assert.IsTrue(db.ContainsKey("lemon"));
            Assert.IsTrue(db.ContainsKey("corn"));
            Assert.IsTrue(db.ContainsKey("breast"));
            Assert.IsTrue(db.ContainsKey("bacon"));
            Assert.IsTrue(db.ContainsKey("pasta"));
            Assert.IsTrue(db.ContainsKey("oil"));
            Assert.IsTrue(db.ContainsKey("vinegar"));
            Assert.IsTrue(db.ContainsKey("salt"));
            Assert.IsTrue(db.ContainsKey("pepper"));

            Assert.IsFalse(db.ContainsKey("unicorns"));
            Assert.IsFalse(db.ContainsKey("rainbows"));
            Assert.IsFalse(db.ContainsKey("antimatter"));

            Assert.IsFalse(db.ContainsKey(""));

            IngredientsDatabase.Inventory Ingredients;

            Assert.IsTrue(db.TryGetValue("garlic", out Ingredients));
            Assert.AreEqual("produce",
                Ingredients.Category);
            Assert.AreEqual(0.67,
                Ingredients.Price);
            Assert.AreEqual(false,
                Ingredients.IsTaxable());
            Assert.AreEqual(true,
                Ingredients.IsWellness());

            Assert.IsTrue(db.TryGetValue("bacon", out Ingredients));
            Assert.AreEqual("meat/poultry",
                Ingredients.Category);
            Assert.AreEqual(0.24,
                Ingredients.Price);
            Assert.AreEqual(true,
                Ingredients.IsTaxable());
            Assert.AreEqual(false,
                Ingredients.IsWellness());

            Assert.IsTrue(db.TryGetValue("vinegar", out Ingredients));
            Assert.AreEqual("pantry",
                Ingredients.Category);
            Assert.AreEqual(1.26,
                Ingredients.Price);
            Assert.AreEqual(true,
                Ingredients.IsTaxable());
            Assert.AreEqual(false,
                Ingredients.IsWellness());
        }

    }
}



