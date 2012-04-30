using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentC;

namespace Tests
{
    [TestClass]
    public class Conditionals
    {
        public FluentCParser_Accessor Evaluator { get; set; }

        [TestInitialize]
        public void Initialize()
        {
            Evaluator = new FluentCParser_Accessor();
        }

        #region Larger Than
        [TestMethod]
        public void TestNumericalLargerThan_Worded()
        {
            Assert.IsTrue((bool)Evaluator.EvaluateExpression("5 is larger than 3"));
            Assert.IsTrue((bool)Evaluator.EvaluateExpression("1 is larger than -1"));
            Assert.IsTrue((bool)Evaluator.EvaluateExpression("0 is larger than -1"));
            Assert.IsTrue((bool)Evaluator.EvaluateExpression("1.0 is larger than -1"));
            Assert.IsTrue((bool)Evaluator.EvaluateExpression("1.0 is larger than -.1"));

            Assert.IsFalse((bool)Evaluator.EvaluateExpression("3 is larger than 5"));
            Assert.IsFalse((bool)Evaluator.EvaluateExpression("-1 is larger than 1"));
            Assert.IsFalse((bool)Evaluator.EvaluateExpression("-1 is larger than 0"));
            Assert.IsFalse((bool)Evaluator.EvaluateExpression("-1 is larger than 1.0"));
            Assert.IsFalse((bool)Evaluator.EvaluateExpression("-1 is larger than 1.0"));

            Assert.IsFalse((bool)Evaluator.EvaluateExpression("1.0 is larger than 1.0"));
            Assert.IsFalse((bool)Evaluator.EvaluateExpression("1 is larger than 1.0"));
            Assert.IsFalse((bool)Evaluator.EvaluateExpression("1.0 is larger than 1"));
            Assert.IsFalse((bool)Evaluator.EvaluateExpression(".1 is larger than .1"));

        }

        [TestMethod]
        public void TestNumericalLargerThan_Symboled()
        {
            Assert.IsTrue((bool)Evaluator.EvaluateExpression("5 > 3"));
            Assert.IsTrue((bool)Evaluator.EvaluateExpression("1 > -1"));
            Assert.IsTrue((bool)Evaluator.EvaluateExpression("0 > -1"));
            Assert.IsTrue((bool)Evaluator.EvaluateExpression("1.0 > -1"));
            Assert.IsTrue((bool)Evaluator.EvaluateExpression("1.0 > -.1"));

            Assert.IsFalse((bool)Evaluator.EvaluateExpression("3 > 5"));
            Assert.IsFalse((bool)Evaluator.EvaluateExpression("-1 > 1"));
            Assert.IsFalse((bool)Evaluator.EvaluateExpression("-1 > 0"));
            Assert.IsFalse((bool)Evaluator.EvaluateExpression("-1 > 1.0"));
            Assert.IsFalse((bool)Evaluator.EvaluateExpression("-1 > 1.0"));

            Assert.IsFalse((bool)Evaluator.EvaluateExpression("1.0 > 1.0"));
            Assert.IsFalse((bool)Evaluator.EvaluateExpression("1 > 1.0"));
            Assert.IsFalse((bool)Evaluator.EvaluateExpression("1.0 > 1"));
            Assert.IsFalse((bool)Evaluator.EvaluateExpression(".1 > .1"));
        }

        [TestMethod]
        public void TestStringLargerThan_Worded()
        {
            Evaluator.Run("Let a be \"a\". Let b be \"b\". Let c be \"c\". Let dog be \"dog\". Let cat be \"cat\".");
            Assert.IsTrue((bool)Evaluator.EvaluateExpression("b is larger than a"));
            Assert.IsTrue((bool)Evaluator.EvaluateExpression("c is larger than a"));
            Assert.IsTrue((bool)Evaluator.EvaluateExpression("c is larger than b"));
            Assert.IsTrue((bool)Evaluator.EvaluateExpression("dog is larger than cat"));
            Assert.IsTrue((bool)Evaluator.EvaluateExpression("cat is larger than a"));

            Assert.IsFalse((bool)Evaluator.EvaluateExpression("a is larger than b"));
            Assert.IsFalse((bool)Evaluator.EvaluateExpression("a is larger than c"));
            Assert.IsFalse((bool)Evaluator.EvaluateExpression("b is larger than c"));
            Assert.IsFalse((bool)Evaluator.EvaluateExpression("cat is larger than dog"));
            Assert.IsFalse((bool)Evaluator.EvaluateExpression("a is larger than cat"));

            Assert.IsFalse((bool)Evaluator.EvaluateExpression("a is larger than a"));
            Assert.IsFalse((bool)Evaluator.EvaluateExpression("b is larger than b"));
            Assert.IsFalse((bool)Evaluator.EvaluateExpression("c is larger than c"));
            Assert.IsFalse((bool)Evaluator.EvaluateExpression("cat is larger than cat"));

        }

        [TestMethod]
        public void TestStringLargerThan_Symboled()
        {
            Evaluator.Run("Let a be \"a\". Let b be \"b\". Let c be \"c\". Let dog be \"dog\". Let cat be \"cat\".");
            Assert.IsTrue((bool)Evaluator.EvaluateExpression("b > a"));
            Assert.IsTrue((bool)Evaluator.EvaluateExpression("c > a"));
            Assert.IsTrue((bool)Evaluator.EvaluateExpression("c > b"));
            Assert.IsTrue((bool)Evaluator.EvaluateExpression("dog > cat"));
            Assert.IsTrue((bool)Evaluator.EvaluateExpression("cat > a"));

            Assert.IsFalse((bool)Evaluator.EvaluateExpression("a > b"));
            Assert.IsFalse((bool)Evaluator.EvaluateExpression("a > c"));
            Assert.IsFalse((bool)Evaluator.EvaluateExpression("b > c"));
            Assert.IsFalse((bool)Evaluator.EvaluateExpression("cat > dog"));
            Assert.IsFalse((bool)Evaluator.EvaluateExpression("a > cat"));

            Assert.IsFalse((bool)Evaluator.EvaluateExpression("a > a"));
            Assert.IsFalse((bool)Evaluator.EvaluateExpression("b > b"));
            Assert.IsFalse((bool)Evaluator.EvaluateExpression("c > c"));
            Assert.IsFalse((bool)Evaluator.EvaluateExpression("cat > cat"));
        }
        #endregion

        #region Smaller Than
        [TestMethod]
        public void TestNumericalSmallerThan_Worded()
        {
            Assert.IsFalse((bool)Evaluator.EvaluateExpression("5 is smaller than 3"));
            Assert.IsFalse((bool)Evaluator.EvaluateExpression("1 is smaller than -1"));
            Assert.IsFalse((bool)Evaluator.EvaluateExpression("0 is smaller than -1"));
            Assert.IsFalse((bool)Evaluator.EvaluateExpression("1.0 is smaller than -1"));
            Assert.IsFalse((bool)Evaluator.EvaluateExpression("1.0 is smaller than -.1"));

            Assert.IsTrue((bool)Evaluator.EvaluateExpression("3 is smaller than 5"));
            Assert.IsTrue((bool)Evaluator.EvaluateExpression("-1 is smaller than 1"));
            Assert.IsTrue((bool)Evaluator.EvaluateExpression("-1 is smaller than 0"));
            Assert.IsTrue((bool)Evaluator.EvaluateExpression("-1 is smaller than 1.0"));
            Assert.IsTrue((bool)Evaluator.EvaluateExpression("-1 is smaller than 1.0"));

            Assert.IsTrue((bool)Evaluator.EvaluateExpression("1.0 is smaller than 1.0"));
            Assert.IsTrue((bool)Evaluator.EvaluateExpression("1 is smaller than 1.0"));
            Assert.IsTrue((bool)Evaluator.EvaluateExpression("1.0 is smaller than 1"));
            Assert.IsTrue((bool)Evaluator.EvaluateExpression(".1 is smaller than .1"));

        }

        [TestMethod]
        public void TestNumericalSmallerThan_Symboled()
        {
            Assert.IsFalse((bool)Evaluator.EvaluateExpression("5 < 3"));
            Assert.IsFalse((bool)Evaluator.EvaluateExpression("1 < -1"));
            Assert.IsFalse((bool)Evaluator.EvaluateExpression("0 < -1"));
            Assert.IsFalse((bool)Evaluator.EvaluateExpression("1.0 < -1"));
            Assert.IsFalse((bool)Evaluator.EvaluateExpression("1.0 < -.1"));

            Assert.IsTrue((bool)Evaluator.EvaluateExpression("3 < 5"));
            Assert.IsTrue((bool)Evaluator.EvaluateExpression("-1 < 1"));
            Assert.IsTrue((bool)Evaluator.EvaluateExpression("-1 < 0"));
            Assert.IsTrue((bool)Evaluator.EvaluateExpression("-1 < 1.0"));
            Assert.IsTrue((bool)Evaluator.EvaluateExpression("-1 < 1.0"));

            Assert.IsTrue((bool)Evaluator.EvaluateExpression("1.0 < 1.0"));
            Assert.IsTrue((bool)Evaluator.EvaluateExpression("1 < 1.0"));
            Assert.IsTrue((bool)Evaluator.EvaluateExpression("1.0 < 1"));
            Assert.IsTrue((bool)Evaluator.EvaluateExpression(".1 < .1"));
        }

        [TestMethod]
        public void TestStringSmallerThan_Worded()
        {
            Evaluator.Run("Let a be \"a\". Let b be \"b\". Let c be \"c\". Let dog be \"dog\". Let cat be \"cat\".");
            Assert.IsFalse((bool)Evaluator.EvaluateExpression("b is smaller than a"));
            Assert.IsFalse((bool)Evaluator.EvaluateExpression("c is smaller than a"));
            Assert.IsFalse((bool)Evaluator.EvaluateExpression("c is smaller than b"));
            Assert.IsFalse((bool)Evaluator.EvaluateExpression("dog is smaller than cat"));
            Assert.IsFalse((bool)Evaluator.EvaluateExpression("cat is smaller than a"));

            Assert.IsTrue((bool)Evaluator.EvaluateExpression("a is smaller than b"));
            Assert.IsTrue((bool)Evaluator.EvaluateExpression("a is smaller than c"));
            Assert.IsTrue((bool)Evaluator.EvaluateExpression("b is smaller than c"));
            Assert.IsTrue((bool)Evaluator.EvaluateExpression("cat is smaller than dog"));
            Assert.IsTrue((bool)Evaluator.EvaluateExpression("a is smaller than cat"));

            Assert.IsTrue((bool)Evaluator.EvaluateExpression("a is smaller than a"));
            Assert.IsTrue((bool)Evaluator.EvaluateExpression("b is smaller than b"));
            Assert.IsTrue((bool)Evaluator.EvaluateExpression("c is smaller than c"));
            Assert.IsTrue((bool)Evaluator.EvaluateExpression("cat is smaller than cat"));

        }

        [TestMethod]
        public void TestStringSmallerThan_Symboled()
        {
            Evaluator.Run("Let a be \"a\". Let b be \"b\". Let c be \"c\". Let dog be \"dog\". Let cat be \"cat\".");
            Assert.IsFalse((bool)Evaluator.EvaluateExpression("b < a"));
            Assert.IsFalse((bool)Evaluator.EvaluateExpression("c < a"));
            Assert.IsFalse((bool)Evaluator.EvaluateExpression("c < b"));
            Assert.IsFalse((bool)Evaluator.EvaluateExpression("dog < cat"));
            Assert.IsFalse((bool)Evaluator.EvaluateExpression("cat < a"));

            Assert.IsTrue((bool)Evaluator.EvaluateExpression("a < b"));
            Assert.IsTrue((bool)Evaluator.EvaluateExpression("a < c"));
            Assert.IsTrue((bool)Evaluator.EvaluateExpression("b < c"));
            Assert.IsTrue((bool)Evaluator.EvaluateExpression("cat < dog"));
            Assert.IsTrue((bool)Evaluator.EvaluateExpression("a < cat"));

            Assert.IsTrue((bool)Evaluator.EvaluateExpression("a < a"));
            Assert.IsTrue((bool)Evaluator.EvaluateExpression("b < b"));
            Assert.IsTrue((bool)Evaluator.EvaluateExpression("c < c"));
            Assert.IsTrue((bool)Evaluator.EvaluateExpression("cat < cat"));
        }

        #endregion

        [TestMethod]
        public void TestNumericalSameAs_Worded()
        {
            Assert.IsFalse((bool)Evaluator.EvaluateExpression("5 is the same as 3"));
            Assert.IsFalse((bool)Evaluator.EvaluateExpression("1 is the same as .1"));
            Assert.IsFalse((bool)Evaluator.EvaluateExpression("-1 is the same as -1.2"));
            Assert.IsFalse((bool)Evaluator.EvaluateExpression("1.0 is the same as -1"));
            Assert.IsFalse((bool)Evaluator.EvaluateExpression("1.0 is the same as -.1"));

            Assert.IsTrue((bool)Evaluator.EvaluateExpression("5 is the same as 5"));
            Assert.IsTrue((bool)Evaluator.EvaluateExpression("-1 is the same as -1"));
            Assert.IsTrue((bool)Evaluator.EvaluateExpression("-1 is the same as -1.0"));
            Assert.IsTrue((bool)Evaluator.EvaluateExpression("-1.0 is the same as -1.0"));
            Assert.IsTrue((bool)Evaluator.EvaluateExpression("-.1 is the same as -0.1"));

        }

        [TestMethod]
        public void TestNumericalSameAs_Symboled()
        {
            Assert.IsFalse((bool)Evaluator.EvaluateExpression("5 = 3"));
            Assert.IsFalse((bool)Evaluator.EvaluateExpression("1 = -1"));
            Assert.IsFalse((bool)Evaluator.EvaluateExpression("0 = -1"));
            Assert.IsFalse((bool)Evaluator.EvaluateExpression("1.0 = -1"));
            Assert.IsFalse((bool)Evaluator.EvaluateExpression("1.0 = -.1"));

            Assert.IsTrue((bool)Evaluator.EvaluateExpression("1.00 = 1.0"));
            Assert.IsTrue((bool)Evaluator.EvaluateExpression("1 = 1.0"));
            Assert.IsTrue((bool)Evaluator.EvaluateExpression("1.0 = 1"));
            Assert.IsTrue((bool)Evaluator.EvaluateExpression(".1 = .1"));
        }

        [TestMethod]
        public void TestStringSameAs_Worded()
        {
            Evaluator.Run("Let a be \"a\". Let b be \"b\". Let c be \"c\". Let dog be \"dog\". Let cat be \"cat\".");
            Assert.IsFalse((bool)Evaluator.EvaluateExpression("b is the same as a"));
            Assert.IsFalse((bool)Evaluator.EvaluateExpression("c is the same as a"));
            Assert.IsFalse((bool)Evaluator.EvaluateExpression("c is the same as b"));
            Assert.IsFalse((bool)Evaluator.EvaluateExpression("dog is the same as cat"));
            Assert.IsFalse((bool)Evaluator.EvaluateExpression("cat is the same as a"));

            Assert.IsTrue((bool)Evaluator.EvaluateExpression("a is the same as a"));
            Assert.IsTrue((bool)Evaluator.EvaluateExpression("b is the same as b"));
            Assert.IsTrue((bool)Evaluator.EvaluateExpression("c is the same as c"));
            Assert.IsTrue((bool)Evaluator.EvaluateExpression("cat is the same as cat"));

        }

        [TestMethod]
        public void TestStringSameAs_Symboled()
        {
            Evaluator.Run("Let a be \"a\". Let b be \"b\". Let c be \"c\". Let dog be \"dog\". Let cat be \"cat\".");
            Assert.IsFalse((bool)Evaluator.EvaluateExpression("b = a"));
            Assert.IsFalse((bool)Evaluator.EvaluateExpression("c = a"));
            Assert.IsFalse((bool)Evaluator.EvaluateExpression("c = b"));
            Assert.IsFalse((bool)Evaluator.EvaluateExpression("dog = cat"));
            Assert.IsFalse((bool)Evaluator.EvaluateExpression("cat = a"));

            Assert.IsTrue((bool)Evaluator.EvaluateExpression("a = a"));
            Assert.IsTrue((bool)Evaluator.EvaluateExpression("b = b"));
            Assert.IsTrue((bool)Evaluator.EvaluateExpression("c = c"));
            Assert.IsTrue((bool)Evaluator.EvaluateExpression("cat = cat"));
        }
    }
}
