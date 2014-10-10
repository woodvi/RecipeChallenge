using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RecipeChallenge;
using System.Collections.Generic;

namespace UnitTestProject
{
    [TestClass]
    public class RecipeTest
    {
        [TestMethod]
        public void ReadIngrediant()
        {
            Recipe.Ingredient i;
            String text;

            text = "Recipe 1 ";
            i = Recipe.ReadIngrediant(text);
            Assert.AreEqual(
                "recipe 1",
                 i.Description.Trim().ToLower(),
                String.Format("ReadIngrediant: {0}", text)
                );


            text = " 1 garlic clove ";
            i = Recipe.ReadIngrediant(text);
            Assert.AreEqual(
                 "garlic clove",
                 i.Description.Trim().ToLower(),
                String.Format("ReadIngrediant: {0}", text)
                 );


            text = " 1 lemon ";
            i = Recipe.ReadIngrediant(text);
            Assert.AreEqual(
                 "lemon",
                 i.Description.Trim().ToLower(),
                String.Format("ReadIngrediant: {0}", text)
                 );


            text = " 3/4 cup olive oil ";
            i = Recipe.ReadIngrediant(text);
            Assert.AreEqual(
                 "cup olive oil",
                 i.Description.Trim().ToLower(),
                String.Format("ReadIngrediant: {0}", text)
                 );


            text = " 3/4 teaspoons of salt ";
            i = Recipe.ReadIngrediant(text);
            Assert.AreEqual(
                 "teaspoons of salt",
                 i.Description.Trim().ToLower(),
                String.Format("ReadIngrediant: {0}", text)
                 );


            text = " 1/2 teaspoons of pepper ";
            i = Recipe.ReadIngrediant(text);
            Assert.AreEqual(
                "teaspoons of pepper",
                i.Description.Trim().ToLower(),
               String.Format("ReadIngrediant: {0}", text)
                );

        }

        [TestMethod]
        public void StringLooksLikeTitle()
        {

            String text;

            // titles: true

            text = "Recipe 1 ";
            Assert.IsTrue(
                Recipe.StringLooksLikeTitle(text),
                "StringLooksLikeTitle");

            text = "Quiche Lorraine";
            Assert.IsTrue(
                Recipe.StringLooksLikeTitle(text),
                "StringLooksLikeTitle");

            // ingredients: false

            text = " 1 garlic clove ";
            Assert.IsFalse(
                Recipe.StringLooksLikeTitle(text),
                "StringLooksLikeTitle");

            text = " 1 lemon ";
            Assert.IsFalse(
                Recipe.StringLooksLikeTitle(text),
                "StringLooksLikeTitle");

            text = " 3/4 cup olive oil ";
            Assert.IsFalse(
                Recipe.StringLooksLikeTitle(text),
                "StringLooksLikeTitle");

            text = " 3/4 teaspoons of salt ";
            Assert.IsFalse(
                Recipe.StringLooksLikeTitle(text),
                "StringLooksLikeTitle");

            text = " 1/2 teaspoons of pepper ";
            Assert.IsFalse(
                Recipe.StringLooksLikeTitle(text),
                "StringLooksLikeTitle");

            // blank lines: false

            text = "  ";
            Assert.IsFalse(
                Recipe.StringLooksLikeTitle(text),
                "StringLooksLikeTitle");

            text = "";
            Assert.IsFalse(
                Recipe.StringLooksLikeTitle(text),
                "StringLooksLikeTitle");

            text = " \t ";
            Assert.IsFalse(
                Recipe.StringLooksLikeTitle(text),
                "StringLooksLikeTitle");

            text = null;

            // instructions: false

            text = @"Process them in a food processor to make crumbs. Bind them together with melted butter and flavor them with cinnamon if you like. Press them into a pie pan as you would a graham cracker crust or line the bottom of a spring-form pan (and up the sides) for a cheesecake crust. Coarsely crumble stale cookies and toast them in the oven for 5 minutes, then sprinkle them over ice cream as a topping. Make a 1/4-inch layer of cookie crumbs at the bottom of a refrigerator cake; top with pudding, mousse or melted ice cream, then freeze or refrigerate until serving. Sugar cookie dough may also be rolled out (before being baked) and used to line sweet pies such as custard, ricotta pie, pudding pies, pumpkin pies or custards.";
            Assert.IsFalse(
                Recipe.StringLooksLikeTitle(text),
                "StringLooksLikeIngredient");
            text = @"You can make this ahead of time and store it in a sealed container in the refrigerator so that it will be ready for breakfast each morning.Whisk together flour, baking powder, baking soda and salt.Whip egg whites, set aside. Beat egg yolks with sugar and stir in milk. Beat in flour mixture. Add melted butter or shortening.When thoroughly combined, fold in beaten egg whites.If batter is too thin, add a tablespoon of flour; if batter is too thick, add a tablespoon of extra milk, adjusting the batter until it has the consistency of very heavy cream. The batter may thicken upon standing and may require adjustment again.Serve dotted with butter, drizzled with maple syrup or topped with a spoonful of marmalade or fruit.Variation: Once batter is on the griddle, add fruit such as blueberries, strawberries or bananas.";
            Assert.IsFalse(
                Recipe.StringLooksLikeTitle(text),
                "StringLooksLikeIngredient");

        }

        [TestMethod]
        public void StringLooksLikeIngredient()
        {

            String text;

            // titles: false

            text = "Recipe 1 ";
            Assert.IsFalse(
                Recipe.StringLooksLikeIngredient(text),
                "StringLooksLikeIngredient");

            text = "Quiche Lorraine";
            Assert.IsFalse(
                Recipe.StringLooksLikeIngredient(text),
                "StringLooksLikeIngredient");

            // ingredients: true

            text = " 1 garlic clove ";
            Assert.IsTrue(
                Recipe.StringLooksLikeIngredient(text),
                "StringLooksLikeIngredient");

            text = " 1 lemon ";
            Assert.IsTrue(
                Recipe.StringLooksLikeIngredient(text),
                "StringLooksLikeIngredient");

            text = " 3/4 cup olive oil ";
            Assert.IsTrue(
                Recipe.StringLooksLikeIngredient(text),
                "StringLooksLikeIngredient");

            text = " 3/4 teaspoons of salt ";
            Assert.IsTrue(
                Recipe.StringLooksLikeIngredient(text),
                "StringLooksLikeIngredient");

            text = " 1/2 teaspoons of pepper ";
            Assert.IsTrue(
                Recipe.StringLooksLikeIngredient(text),
                "StringLooksLikeIngredient");

            // blank lines: false

            text = "  ";
            Assert.IsFalse(
                Recipe.StringLooksLikeIngredient(text),
                "StringLooksLikeIngredient");

            text = "";
            Assert.IsFalse(
                Recipe.StringLooksLikeIngredient(text),
                "StringLooksLikeIngredient");

            text = " \t ";
            Assert.IsFalse(
                Recipe.StringLooksLikeIngredient(text),
                "StringLooksLikeIngredient");

            text = null;

            // instructions: false

            text = @"Process them in a food processor to make crumbs. Bind them together with melted butter and flavor them with cinnamon if you like. Press them into a pie pan as you would a graham cracker crust or line the bottom of a spring-form pan (and up the sides) for a cheesecake crust. Coarsely crumble stale cookies and toast them in the oven for 5 minutes, then sprinkle them over ice cream as a topping. Make a 1/4-inch layer of cookie crumbs at the bottom of a refrigerator cake; top with pudding, mousse or melted ice cream, then freeze or refrigerate until serving. Sugar cookie dough may also be rolled out (before being baked) and used to line sweet pies such as custard, ricotta pie, pudding pies, pumpkin pies or custards.";
            Assert.IsFalse(
                Recipe.StringLooksLikeIngredient(text),
                "StringLooksLikeIngredient");
            text = @"You can make this ahead of time and store it in a sealed container in the refrigerator so that it will be ready for breakfast each morning.Whisk together flour, baking powder, baking soda and salt.Whip egg whites, set aside. Beat egg yolks with sugar and stir in milk. Beat in flour mixture. Add melted butter or shortening.When thoroughly combined, fold in beaten egg whites.If batter is too thin, add a tablespoon of flour; if batter is too thick, add a tablespoon of extra milk, adjusting the batter until it has the consistency of very heavy cream. The batter may thicken upon standing and may require adjustment again.Serve dotted with butter, drizzled with maple syrup or topped with a spoonful of marmalade or fruit.Variation: Once batter is on the griddle, add fruit such as blueberries, strawberries or bananas.";
            Assert.IsFalse(
                Recipe.StringLooksLikeIngredient(text),
                "StringLooksLikeIngredient");

        }

        [TestMethod]
        public void StringLooksLikeBlank()
        {

            String text;

            // titles: true

            text = "Recipe 1 ";
            Assert.IsFalse(
                Recipe.StringLooksLikeBlank(text),
                "StringLooksLikeBlank");

            text = "Quiche Lorraine";
            Assert.IsFalse(
                Recipe.StringLooksLikeBlank(text),
                "StringLooksLikeBlank");

            // ingredients: false

            text = " 1 garlic clove ";
            Assert.IsFalse(
                Recipe.StringLooksLikeBlank(text),
                "StringLooksLikeBlank");

            text = " 1 lemon ";
            Assert.IsFalse(
                Recipe.StringLooksLikeBlank(text),
                "StringLooksLikeBlank");

            text = " 3/4 cup olive oil ";
            Assert.IsFalse(
                Recipe.StringLooksLikeBlank(text),
                "StringLooksLikeBlank");

            text = " 3/4 teaspoons of salt ";
            Assert.IsFalse(
                Recipe.StringLooksLikeBlank(text),
                "StringLooksLikeBlank");

            text = " 1/2 teaspoons of pepper ";
            Assert.IsFalse(
                Recipe.StringLooksLikeBlank(text),
                "StringLooksLikeBlank");

            // blank lines: false

            text = "  ";
            Assert.IsTrue(
                Recipe.StringLooksLikeBlank(text),
                "StringLooksLikeBlank");

            text = "";
            Assert.IsTrue(
                Recipe.StringLooksLikeBlank(text),
                "StringLooksLikeBlank");

            text = " \t ";
            Assert.IsTrue(
                Recipe.StringLooksLikeBlank(text),
                "StringLooksLikeBlank");

            text = null;
            Assert.IsTrue(
                Recipe.StringLooksLikeBlank(text),
                "StringLooksLikeBlank");

            // instructions: false

            text = @"Process them in a food processor to make crumbs. Bind them together with melted butter and flavor them with cinnamon if you like. Press them into a pie pan as you would a graham cracker crust or line the bottom of a spring-form pan (and up the sides) for a cheesecake crust. Coarsely crumble stale cookies and toast them in the oven for 5 minutes, then sprinkle them over ice cream as a topping. Make a 1/4-inch layer of cookie crumbs at the bottom of a refrigerator cake; top with pudding, mousse or melted ice cream, then freeze or refrigerate until serving. Sugar cookie dough may also be rolled out (before being baked) and used to line sweet pies such as custard, ricotta pie, pudding pies, pumpkin pies or custards.";
            Assert.IsFalse(
                Recipe.StringLooksLikeBlank(text),
                "StringLooksLikeIngredient");
            text = @"You can make this ahead of time and store it in a sealed container in the refrigerator so that it will be ready for breakfast each morning.Whisk together flour, baking powder, baking soda and salt.Whip egg whites, set aside. Beat egg yolks with sugar and stir in milk. Beat in flour mixture. Add melted butter or shortening.When thoroughly combined, fold in beaten egg whites.If batter is too thin, add a tablespoon of flour; if batter is too thick, add a tablespoon of extra milk, adjusting the batter until it has the consistency of very heavy cream. The batter may thicken upon standing and may require adjustment again.Serve dotted with butter, drizzled with maple syrup or topped with a spoonful of marmalade or fruit.Variation: Once batter is on the griddle, add fruit such as blueberries, strawberries or bananas.";
            Assert.IsFalse(
                Recipe.StringLooksLikeBlank(text),
                "StringLooksLikeIngredient");

        }

        [TestMethod]
        public void StringLooksLikeInstructions()
        {

            String text;

            // titles: false (maybe: it's harder to tell instructions from ingredients)

            text = "Recipe 1 ";
            Assert.IsFalse(
                Recipe.StringLooksLikeInstructions(text),
                "StringLooksLikeInstructions");

            text = "Quiche Lorraine";
            Assert.IsFalse(
                Recipe.StringLooksLikeInstructions(text),
                "StringLooksLikeInstructions");

            // ingredients: true

            text = " 1 garlic clove ";
            Assert.IsFalse(
                Recipe.StringLooksLikeInstructions(text),
                "StringLooksLikeInstructions");

            text = " 1 lemon ";
            Assert.IsFalse(
                Recipe.StringLooksLikeInstructions(text),
                "StringLooksLikeInstructions");

            text = " 3/4 cup olive oil ";
            Assert.IsFalse(
                Recipe.StringLooksLikeInstructions(text),
                "StringLooksLikeInstructions");

            text = " 3/4 teaspoons of salt ";
            Assert.IsFalse(
                Recipe.StringLooksLikeInstructions(text),
                "StringLooksLikeInstructions");

            text = " 1/2 teaspoons of pepper ";
            Assert.IsFalse(
                Recipe.StringLooksLikeInstructions(text),
                "StringLooksLikeInstructions");

            // blank lines: false

            text = "  ";
            Assert.IsFalse(
                Recipe.StringLooksLikeInstructions(text),
                "StringLooksLikeInstructions");

            text = "";
            Assert.IsFalse(
                Recipe.StringLooksLikeInstructions(text),
                "StringLooksLikeInstructions");

            text = " \t ";
            Assert.IsFalse(
                Recipe.StringLooksLikeInstructions(text),
                "StringLooksLikeInstructions");

            text = null;

            // instructions: true

            text = @"Process them in a food processor to make crumbs. Bind them together with melted butter and flavor them with cinnamon if you like. Press them into a pie pan as you would a graham cracker crust or line the bottom of a spring-form pan (and up the sides) for a cheesecake crust. Coarsely crumble stale cookies and toast them in the oven for 5 minutes, then sprinkle them over ice cream as a topping. Make a 1/4-inch layer of cookie crumbs at the bottom of a refrigerator cake; top with pudding, mousse or melted ice cream, then freeze or refrigerate until serving. Sugar cookie dough may also be rolled out (before being baked) and used to line sweet pies such as custard, ricotta pie, pudding pies, pumpkin pies or custards.";
            Assert.IsTrue(
                Recipe.StringLooksLikeInstructions(text),
                "StringLooksLikeInstructions");
            text = @"You can make this ahead of time and store it in a sealed container in the refrigerator so that it will be ready for breakfast each morning.Whisk together flour, baking powder, baking soda and salt.Whip egg whites, set aside. Beat egg yolks with sugar and stir in milk. Beat in flour mixture. Add melted butter or shortening.When thoroughly combined, fold in beaten egg whites.If batter is too thin, add a tablespoon of flour; if batter is too thick, add a tablespoon of extra milk, adjusting the batter until it has the consistency of very heavy cream. The batter may thicken upon standing and may require adjustment again.Serve dotted with butter, drizzled with maple syrup or topped with a spoonful of marmalade or fruit.Variation: Once batter is on the griddle, add fruit such as blueberries, strawberries or bananas.";
            Assert.IsTrue(
                Recipe.StringLooksLikeInstructions(text),
                "StringLooksLikeInstructions");


        }


        [TestMethod]
        public void AddIngredient()
        {
            Recipe r = new Recipe("Hello World");
            Recipe.Ingredient i1 = new Recipe.Ingredient(1.1, "yak");
            Recipe.Ingredient i2 = new Recipe.Ingredient(1.2, "plover");
            Recipe.Ingredient i3 = new Recipe.Ingredient(1.3, "xyzzy");
            r.Add("a", i1);
            r.Add("b", i2);
            r.Add("c", i3);

            Assert.AreEqual(
                "Hello World",
                r.Title);

            double qty;
            String Description;
            Recipe.Ingredient i0;
                
            Assert.IsTrue(
                r.TryGetQuantity("a", out qty ));
            Assert.AreEqual(
                1.1, 
                qty);

            Assert.IsTrue(
                r.TryGetDescription("b", out Description));
            Assert.AreEqual(
                "plover",
                Description);

            Assert.IsTrue(
                r.TryGetValue("c", out i0));
            Assert.AreEqual(
                1.3,
                i0.Quantity);

            Assert.IsFalse(
                r.TryGetDescription("d", out Description));

        }

        [TestMethod]
        public void CalculateKey()
        {
            String Line;
            String[] Words = new String[] { "meat", "vegetable", "starch", "spice" };
            ICollection<String> IngredientsCorpus = new List<String>(Words);

            Line = "1 meat tranche";
            Assert.AreEqual(
                "meat",
                Recipe.CalculateKey(Line, IngredientsCorpus));

            Line = "1 vegetable";
            Assert.AreEqual(
                "vegetable",
                Recipe.CalculateKey(Line, IngredientsCorpus));

            Line = "starchy stuff";
            Assert.AreEqual(
                "starch",
                Recipe.CalculateKey(Line, IngredientsCorpus));

            Line = "1 dash of spice";
            Assert.AreEqual(
                "spice",
                Recipe.CalculateKey(Line, IngredientsCorpus));

        }

        [TestMethod]
        public void ReadRecipe()
        {
            String TextData = "Recipe 1 \n" +
" 1 garlic clove \n" +
" 1 lemon \n" +
" 3/4 cup olive oil \n" +
" 3/4 teaspoons of salt \n" +
" 1/2 teaspoons of pepper \n";

            String[] Words = new String[] { "garlic", "lemon", "oil", "salt", "pepper" };
            ICollection<String> IngredientsCorpus = new List<String>(Words);

            // read the recipe    
            System.IO.StringReader s = new System.IO.StringReader(TextData);
            Recipe r = Recipe.ReadRecipe(s, IngredientsCorpus);

            // check the name of the recipe
            Assert.AreEqual("Recipe 1", r.Title);

            // check what's in the recipe
            double Quantity;
            String Description;
            Recipe.Ingredient Ingredient;

            Assert.IsTrue(r.TryGetQuantity("garlic", out Quantity));
            Assert.AreEqual( 1, Quantity );

            Assert.IsTrue(r.TryGetQuantity("oil", out Quantity));
            Assert.AreEqual(0.75, Quantity);

            Assert.IsTrue(r.TryGetDescription("lemon", out Description));
            Assert.AreEqual("lemon", Description);

            Assert.IsTrue(r.TryGetDescription("salt", out Description));
            Assert.AreEqual("teaspoons of salt", Description);

            Assert.IsTrue(r.TryGetValue("pepper", out Ingredient));
            Assert.AreEqual(0.5, Ingredient.Quantity);
            Assert.AreEqual("teaspoons of pepper", Ingredient.Description);

            Assert.IsFalse(r.TryGetValue("unicorns", out Ingredient));      // our recipe has no unicorns
        }

    }
}
