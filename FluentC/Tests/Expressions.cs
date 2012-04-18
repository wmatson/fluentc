using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentC;

namespace Tests
{
    [TestClass]
    public class Expressions
    {
        private const decimal TOLERANCE = .001M;

        [TestMethod]
        public void TestEvaluateExpressionVariableRecall()
        {
            var evaluator = new FluentCParser_Accessor();
            evaluator.Run("Let x be 2.");
            Assert.AreEqual(2M, evaluator.EvaluateExpression("x"));
            evaluator.Run("Let x be \"two\".");
            Assert.AreEqual("two", evaluator.EvaluateExpression("x"));
        }

        [TestMethod]
        public void TestEvaluateSimpleExpression()
        {
            var evaluator = new FluentCParser_Accessor();
            Assert.AreEqual(2M, evaluator.EvaluateExpression("2"));
            Assert.AreEqual(2.3M, evaluator.EvaluateExpression("2.3"));
            Assert.AreEqual("Hello", evaluator.EvaluateExpression("\"Hello\""));
            Assert.AreEqual("Hello world", evaluator.EvaluateExpression("\"Hello world\""));
        }

        [TestMethod]
        public void TestEvaluateAdditionExpression()
        {
            var evaluator = new FluentCParser_Accessor();
            Assert.AreEqual(2M, evaluator.EvaluateExpression("1 + 1"));
            Assert.AreEqual(2.3M, evaluator.EvaluateExpression("2 + .3"));
            Assert.AreEqual(2.3M, evaluator.EvaluateExpression("2 + 0.3"));
            Assert.AreEqual("Hello", evaluator.EvaluateExpression("\"Hell\" + \"o\""));
        }

        [TestMethod]
        public void TestEvaluateSubtractionExpression()
        {
            var evaluator = new FluentCParser_Accessor();
            Assert.AreEqual(0M, evaluator.EvaluateExpression("1 - 1"));
            Assert.AreEqual(1M, evaluator.EvaluateExpression("1 - 0"));
            Assert.AreEqual(1.7M, evaluator.EvaluateExpression("2 - .3"));
            Assert.AreEqual(1.7M, evaluator.EvaluateExpression("2 - 0.3"));
            Assert.AreEqual(2M, evaluator.EvaluateExpression("2.3 - 0.3"));
        }

        [TestMethod]
        public void TestEvaluateMultiplicationExpression()
        {
            var evaluator = new FluentCParser_Accessor();
            Assert.AreEqual(1M, evaluator.EvaluateExpression("1 * 1"));
            Assert.AreEqual(0M, evaluator.EvaluateExpression("1 * 0"));
            Assert.AreEqual(.6M, evaluator.EvaluateExpression("2 * .3"));
            Assert.AreEqual(.6M, evaluator.EvaluateExpression("2 * 0.3"));
            Assert.AreEqual(.69M, evaluator.EvaluateExpression("2.3 * 0.3"));
        }

        [TestMethod]
        public void TestEvaluateDivisionExpression()
        {
            var evaluator = new FluentCParser_Accessor();
            Assert.AreEqual(1M, evaluator.EvaluateExpression("1 / 1"));
            Assert.IsTrue(TOLERANCE > Math.Abs(decimal.Parse(evaluator.EvaluateExpression("2 / .3").ToString()) - (2M / .3M)));
            Assert.AreEqual(4M, evaluator.EvaluateExpression("20 / 5"));
        }

        [TestMethod]
        public void TestEvaluateVariableExpression()
        {
            var evaluator = new FluentCParser_Accessor();
            evaluator.Run("Let x be 1.");
            Assert.AreEqual(2M, evaluator.EvaluateExpression("1 + x"));
            evaluator.Run("Let x be 2.");
            Assert.AreEqual(2.3M, evaluator.EvaluateExpression("x + .3"));
            Assert.AreEqual(2.3M, evaluator.EvaluateExpression("x + 0.3"));
            evaluator.Run("Let x be \"o\".");
            Assert.AreEqual("Hello", evaluator.EvaluateExpression("\"Hell\" + x"));
            evaluator.Run("Let y be \" World\".");
            Assert.AreEqual("o World", evaluator.EvaluateExpression("x + y"));
        }

        [TestMethod]
        public void TestEvaluateNumericalExpression() {
            Assert.AreEqual(4M, FluentCParser_Accessor.EvaluateNumericalExpression("1 + 3"));
            Assert.AreEqual(4M, FluentCParser_Accessor.EvaluateNumericalExpression("(1 + 3)"));
            Assert.AreEqual(4M, FluentCParser_Accessor.EvaluateNumericalExpression("(4)"));
            Assert.AreEqual(-4M, FluentCParser_Accessor.EvaluateNumericalExpression("(-4)"));
            Assert.AreEqual(-2M, FluentCParser_Accessor.EvaluateNumericalExpression("(1 + -3)"));
            Assert.AreEqual(-2M, FluentCParser_Accessor.EvaluateNumericalExpression("1 + -3"));
            Assert.AreEqual(-2.2M, FluentCParser_Accessor.EvaluateNumericalExpression("1 + -3.2"));
            Assert.AreEqual(1.2M, FluentCParser_Accessor.EvaluateNumericalExpression("1 + .2"));
            Assert.AreEqual(1.2M, FluentCParser_Accessor.EvaluateNumericalExpression("1 + 0.2"));
            Assert.AreEqual(6M, FluentCParser_Accessor.EvaluateNumericalExpression("2 * 3"));
            Assert.AreEqual(2/3M, FluentCParser_Accessor.EvaluateNumericalExpression("2 / 3"));
            Assert.AreEqual(-1M, FluentCParser_Accessor.EvaluateNumericalExpression("2 - 3"));
            Assert.AreEqual(7M, FluentCParser_Accessor.EvaluateNumericalExpression("1 + 2 * 3"));
            Assert.AreEqual(2.5M, FluentCParser_Accessor.EvaluateNumericalExpression("1 + 2 * 3 / 4"));
            Assert.AreEqual(-5M, FluentCParser_Accessor.EvaluateNumericalExpression("4 + 3 - 2 * 7 + 5 * 4 / 10"));
            Assert.AreEqual(30M, FluentCParser_Accessor.EvaluateNumericalExpression("1 + 2 + (3 * (4 + 5))"));
        }
    }
}
