using RecipeChallenge;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace UnitTestProject
{
    [TestClass]
    public class RecipeCalculatorTest
    {
        [TestMethod]
        public void RoundToFactor()
        {
            Assert.AreEqual(0.21,
                RecipeCalculator.RoundToFactor(0.19, 0.07),
                0.000001);
            Assert.AreEqual(0.24,
                RecipeCalculator.RoundToFactor(0.19, 0.06),
                0.000001);

            // ordinary integers
            Assert.AreEqual(8,
                RecipeCalculator.RoundToFactor(8, 2),
                0.000001);
            Assert.AreEqual(10,
                RecipeCalculator.RoundToFactor(9, 2),
                0.000001);
            Assert.AreEqual(10,
                RecipeCalculator.RoundToFactor(10, 2),
                0.000001);
            Assert.AreEqual(9,
                RecipeCalculator.RoundToFactor(8, 3),
                0.000001);
            Assert.AreEqual(9,
                RecipeCalculator.RoundToFactor(9, 3),
                0.000001);
            Assert.AreEqual(12,
                RecipeCalculator.RoundToFactor(10, 3),
                0.000001);

            // fractions
            Assert.AreEqual(0.08,
                RecipeCalculator.RoundToFactor(0.08, 0.02),
                0.000001);
            Assert.AreEqual(0.10,
                RecipeCalculator.RoundToFactor(0.09, 0.02),
                0.000001);
            Assert.AreEqual(0.10,
                RecipeCalculator.RoundToFactor(0.10, 0.02),
                0.000001);
            Assert.AreEqual(0.09,
                RecipeCalculator.RoundToFactor(0.08, 0.03),
                0.000001);

            // non-positive numbers
            Assert.AreEqual(9,
                RecipeCalculator.RoundToFactor(9, 0),
                0.000001);
            Assert.AreEqual(-12,
                RecipeCalculator.RoundToFactor(-9, 4),
                0.000001);
            Assert.AreEqual(10,
                RecipeCalculator.RoundToFactor(9, -2),
                0.000001);
            Assert.AreEqual(0,
                RecipeCalculator.RoundToFactor(0, 2),
                0.000001);

        }




        [TestMethod]
        public void CalculateRecipe1()
        {
            String IngredientsText =
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

            String RecipeText = "Recipe 1 \n" +
                " 1 garlic clove \n" +
                " 1 lemon \n" +
                " 3/4 cup olive oil \n" +
                " 3/4 teaspoons of salt \n" +
                " 1/2 teaspoons of pepper \n";


            ICollection<String> IngredientsCorpus;
            IngredientsDatabase db = IngredientsDatabase.ReadIngredientsDatabase(new System.IO.StringReader(IngredientsText), out IngredientsCorpus);

            Recipe Recipe = Recipe.ReadRecipe(new System.IO.StringReader(RecipeText), IngredientsCorpus);

            RecipeCalculator Calculator = new RecipeCalculator(Recipe, db);
            Assert.AreEqual(4.45,
                Calculator.Calculate(),
                0.000001);
            Assert.AreEqual(0.21,
                Calculator.Tax,
                0.000001);
            Assert.AreEqual(-0.11,
                Calculator.WellnessDiscount,
                0.000001);
            Assert.AreEqual(4.45,
                Calculator.Total,
                0.000001);
        }

        [TestMethod]
        public void CalculateRecipe2()
        {
            String IngredientsText =
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

            String RecipeText = "Recipe 2 " + "\n" +
                " 1 garlic clove " + "\n" +
                " 4 chicken breasts " + "\n" +
                " 1/2 cup olive oil " + "\n" +
                " 1/2 cup vinegar " + "\n";

            ICollection<String> IngredientsCorpus;
            IngredientsDatabase db = IngredientsDatabase.ReadIngredientsDatabase(new System.IO.StringReader(IngredientsText), out IngredientsCorpus);

            Recipe Recipe = Recipe.ReadRecipe(new System.IO.StringReader(RecipeText), IngredientsCorpus);

            RecipeCalculator Calculator = new RecipeCalculator(Recipe, db);
            Assert.AreEqual(11.84,
                Calculator.Calculate(),
                0.000001);
            Assert.AreEqual(0.91,
                Calculator.Tax,
                0.000001);
            Assert.AreEqual(-0.09,
                Calculator.WellnessDiscount,
                0.000001);
            Assert.AreEqual(11.84,
                Calculator.Total,
                0.000001);
        }

        [TestMethod]
        public void CalculateRecipe123()
        {
            String IngredientsText =
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

            String RecipeText1 = "Recipe 1 \n" +
                " 1 garlic clove \n" +
                " 1 lemon \n" +
                " 3/4 cup olive oil \n" +
                " 3/4 teaspoons of salt \n" +
                " 1/2 teaspoons of pepper \n";


            Recipe Recipe;
            RecipeCalculator Calculator;
            ICollection<String> IngredientsCorpus;
            IngredientsDatabase db = IngredientsDatabase.ReadIngredientsDatabase(new System.IO.StringReader(IngredientsText), out IngredientsCorpus);

            Recipe = Recipe.ReadRecipe(new System.IO.StringReader(RecipeText1), IngredientsCorpus);
            Calculator = new RecipeCalculator( Recipe, db);
            Assert.AreEqual(4.45,
                Calculator.Calculate(),
                0.000001);
            Assert.AreEqual(0.21,
                Calculator.Tax,
                0.000001);
            Assert.AreEqual(-0.11,
                Calculator.WellnessDiscount,
                0.000001);
            Assert.AreEqual(4.45,
                Calculator.Total,
                0.000001);


            String RecipeText2 = "Recipe 2 " + "\n" +
                " 1 garlic clove " + "\n" +
                " 4 chicken breasts " + "\n" +
                " 1/2 cup olive oil " + "\n" +
                " 1/2 cup vinegar " + "\n";

            Calculator.Recipe = Recipe.ReadRecipe(new System.IO.StringReader(RecipeText2), IngredientsCorpus);
            Assert.AreEqual(11.84,
                Calculator.Calculate(),
                0.000001);
            Assert.AreEqual(0.91,
                Calculator.Tax,
                0.000001);
            Assert.AreEqual(-0.09,
                Calculator.WellnessDiscount,
                0.000001);
            Assert.AreEqual(11.84,
                Calculator.Total,
                0.000001);


            String RecipeText3 = "Recipe 3 " + "\n" +
                " 1 garlic clove " + "\n" +
                " 4 cups of corn " + "\n" +
                " 4 slices of bacon " + "\n" +
                " 8 ounces of pasta " + "\n" +
                " 1/3 cup olive oil " + "\n" +
                " 1 1/4 teaspoons of salt " + "\n" +
                " 3/4 teaspoons of pepper " + "\n";

            
            Calculator.Recipe = Recipe.ReadRecipe(new System.IO.StringReader(RecipeText3), IngredientsCorpus);
            Assert.AreEqual(8.91,
                Calculator.Calculate(),
                0.000001);
            Assert.AreEqual(0.42,
                Calculator.Tax,
                0.000001);
            Assert.AreEqual(-0.07,
                Calculator.WellnessDiscount,
                0.000001);
            Assert.AreEqual(8.91,
                Calculator.Total,
                0.000001);

        }

    }
}
