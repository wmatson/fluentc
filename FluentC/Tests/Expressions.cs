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
        [TestMethod]
        public void TestEvaluateExpressionVariableRecall()
        {
            var evaluator = new FluentCParser_Accessor();
            evaluator.Run("Let x be 2.");
            Assert.AreEqual((decimal)2, evaluator.EvaluateExpression("x"));
            evaluator.Run("Let x be \"two\".");
            Assert.AreEqual("two", evaluator.EvaluateExpression("x"));
        }

        [TestMethod]
        public void TestEvaluateSimpleExpression()
        {
            var evaluator = new FluentCParser_Accessor();
            Assert.AreEqual((decimal)2, evaluator.EvaluateExpression("2"));
            Assert.AreEqual((decimal)2.3, evaluator.EvaluateExpression("2.3"));
            Assert.AreEqual("Hello", evaluator.EvaluateExpression("\"Hello\""));
            Assert.AreEqual("Hello world", evaluator.EvaluateExpression("\"Hello world\""));
        }

        [TestMethod]
        public void TestEvaluateAdditionExpression()
        {
            var evaluator = new FluentCParser_Accessor();
            Assert.AreEqual((decimal)2, evaluator.EvaluateExpression("1 + 1"));
            Assert.AreEqual((decimal)2.3, evaluator.EvaluateExpression("2 + .3"));
            Assert.AreEqual((decimal)2.3, evaluator.EvaluateExpression("2 + 0.3"));
            Assert.AreEqual("Hello", evaluator.EvaluateExpression("\"Hell\" + \"o\""));
        }

        [TestMethod]
        public void TestEvaluateSubtractionExpression()
        {
            var evaluator = new FluentCParser_Accessor();
            Assert.AreEqual((decimal)0, evaluator.EvaluateExpression("1 - 1"));
            Assert.AreEqual((decimal)1, evaluator.EvaluateExpression("1 - 0"));
            Assert.AreEqual((decimal)1.7, evaluator.EvaluateExpression("2 - .3"));
            Assert.AreEqual((decimal)1.7, evaluator.EvaluateExpression("2 - 0.3"));
            Assert.AreEqual((decimal)2, evaluator.EvaluateExpression("2.3 - 0.3"));
        }

        [TestMethod]
        public void TestEvaluateMultiplicationExpression()
        {
            var evaluator = new FluentCParser_Accessor();
            Assert.AreEqual((decimal)1, evaluator.EvaluateExpression("1 * 1"));
            Assert.AreEqual((decimal)0, evaluator.EvaluateExpression("1 * 0"));
            Assert.AreEqual((decimal).6, evaluator.EvaluateExpression("2 * .3"));
            Assert.AreEqual((decimal).6, evaluator.EvaluateExpression("2 * 0.3"));
            Assert.AreEqual((decimal).69, evaluator.EvaluateExpression("2.3 * 0.3"));
        }

        [TestMethod]
        public void TestEvaluateDivisionExpression()
        {
            var evaluator = new FluentCParser_Accessor();
            Assert.AreEqual((decimal)1, evaluator.EvaluateExpression("1 / 1"));
            //Assert.AreEqual("infinity", evaluator.EvaluateExpression("1 / 0"));//TODO find a way to assert failure happens
            Assert.AreEqual((decimal)2 / (decimal).3, evaluator.EvaluateExpression("2 / .3"));
            Assert.AreEqual((decimal)(6 + 2.0 / 3.0), evaluator.EvaluateExpression("2 / 0.3"));
            Assert.AreEqual((decimal)4, evaluator.EvaluateExpression("20 / 5"));
        }

        [TestMethod]
        public void TestEvaluateVariableExpression()
        {
            var evaluator = new FluentCParser_Accessor();
            evaluator.Run("Let x be 1.");
            Assert.AreEqual((decimal)2, evaluator.EvaluateExpression("1 + x"));
            evaluator.Run("Let x be 2.");
            Assert.AreEqual((decimal)2.3, evaluator.EvaluateExpression("x + .3"));
            Assert.AreEqual((decimal)2.3, evaluator.EvaluateExpression("x + 0.3"));
            evaluator.Run("Let x be \"o\".");
            Assert.AreEqual("Hello", evaluator.EvaluateExpression("\"Hell\" + x"));
            evaluator.Run("Let y be \" World\".");
            Assert.AreEqual("o World", evaluator.EvaluateExpression("x + y"));
        }

        [TestMethod]
        public void TestEvaluateNumericalExpression() {
            var evaluator = new FluentCParser_Accessor();
            Assert.AreEqual((decimal)4 , evaluator.EvaluateNumericalExpression("1 + 3"));
            Assert.AreEqual((decimal)4, evaluator.EvaluateNumericalExpression("(1 + 3)"));
            Assert.AreEqual((decimal)4, evaluator.EvaluateNumericalExpression("(4)"));
            Assert.AreEqual((decimal)-4, evaluator.EvaluateNumericalExpression("(-4)"));
            Assert.AreEqual((decimal)-2, evaluator.EvaluateNumericalExpression("(1 + -3)"));
            Assert.AreEqual((decimal)-2, evaluator.EvaluateNumericalExpression("1 + -3"));
            Assert.AreEqual((decimal)-2.2, evaluator.EvaluateNumericalExpression("1 + -3.2"));
            Assert.AreEqual((decimal)1.2, evaluator.EvaluateNumericalExpression("1 + .2"));
            Assert.AreEqual((decimal)1.2, evaluator.EvaluateNumericalExpression("1 + 0.2"));
            Assert.AreEqual((decimal)6, evaluator.EvaluateNumericalExpression("2 * 3"));
            Assert.AreEqual((decimal)2/3, evaluator.EvaluateNumericalExpression("2 / 3"));
            Assert.AreEqual((decimal)-1, evaluator.EvaluateNumericalExpression("2 - 3"));
            Assert.AreEqual((decimal)7, evaluator.EvaluateNumericalExpression("1 + 2 * 3"));
            Assert.AreEqual((decimal)2.5, evaluator.EvaluateNumericalExpression("1 + 2 * 3 / 4"));
            Assert.AreEqual((decimal)-5, evaluator.EvaluateNumericalExpression("4 + 3 - 2 * 7 + 5 * 4 / 10"));
        }
    }
}
