using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RecipeChallenge;


namespace UnitTestProject
{
    [TestClass]
    public class FractionalNumberTest
    {
        [TestMethod]
        public void FractionalNumberConvert()
        {
            Assert.AreEqual(999,
                new FractionalNumber(" 999 "),
                0.001,
                "FractionalNumber: 999"
                );

            Assert.AreEqual(-101,
                new FractionalNumber(" -101 "),
                0.001,
                "FractionalNumber: -101"
                );

            Assert.AreEqual(2.9,
                new FractionalNumber("2.9"),
                0.001,
                "FractionalNumber: 2.9"
                );

            Assert.AreEqual(1.75,
                new FractionalNumber("1 3/4"),
                0.001,
                "FractionalNumber: 1 3/4"
                );

            Assert.AreEqual(2.25,
                new FractionalNumber(" 9/4 "),
                0.001,
                "FractionalNumber: 9/4 "
                );

        }
    }
}
