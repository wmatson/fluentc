using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentCEngine;
using FluentCEngine.Constructs;
using FluentC;

namespace Tests
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class Variables
    {

        private Engine Engine { get; set; }
        private FluentCParser Parser { get; set; }

        public Variables()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestInitialize]
        public void Initialize()
        {
            Engine = new Engine();
            Parser = new FluentCParser(Engine);
        }


        [TestMethod]
        public void TestDeclaration()
        {
            Engine.Declare("Hello");
            Assert.IsTrue(Engine.Exists("Hello"));
            Engine.Declare("Goodbye");
            Assert.IsTrue(Engine.Exists("Goodbye"));
            Engine.Declare("The square root of two");
            Assert.IsTrue(Engine.Exists("The square root of two"));
            Engine.Declare("2");
            Assert.IsTrue(Engine.Exists("2"));
            Engine.Declare("2 peas in a pod");
            Assert.IsTrue(Engine.Exists("2 peas in a pod"));
        }

        [TestMethod]
        public void TestAssignment()
        {
            Engine.Declare("Two");
            Engine.Assign("Two", 2);
            Assert.AreEqual(2, Engine.GetValue("Two"));
            Engine.Declare("Goodbye");
            Engine.Assign("Goodbye", "World");
            Assert.AreEqual("World", Engine.GetValue("Goodbye"));
        }

        [TestMethod]
        public void TestDeletion()
        {
            Engine.Declare("Two");
            Assert.IsTrue(Engine.Exists("Two"));
            Engine.Delete("Two");
            Assert.IsFalse(Engine.Exists("Two"));


        }

        [TestMethod]
        public void TestScriptDeclaration()
        {
            Parser.Run("Let x exist.");
            Assert.IsTrue(Engine.Exists("x"));
            Parser.Run("Let hello world exist.");
            Assert.IsTrue(Engine.Exists("hello world"));
            Parser.Run("Let the square root of two exist.");
            Assert.IsTrue(Engine.Exists("the square root of two"));
            Parser.Run("Let 2 exist.");
            Assert.IsTrue(Engine.Exists("2"));
        }

        [TestMethod]
        public void TestScriptAssignment()
        {
            Parser.Run("Let two be 2.");
            Assert.AreEqual(2, Engine.GetValue("two"));
            Parser.Run("Let two be 3.");
            Assert.AreEqual(3, Engine.GetValue("two"));
            Parser.Run("Let two be \"two\".");
            Assert.AreEqual("two", Engine.GetValue("two"));
            Parser.Run("Let goodbye be \"world\".");
            Assert.AreEqual("world", Engine.GetValue("goodbye"));
            Parser.Run("Let a witty saying be \"in development\".");
            Assert.AreEqual("in development", Engine.GetValue("a witty saying"));
        }

        [TestMethod]
        public void TestScriptDeletion()
        {
            Parser.Run("Let x exist.");
            Assert.IsTrue(Engine.Exists("x"));
            Parser.Run("Forget x.");
            Assert.IsFalse(Engine.Exists("x"));

            Parser.Run("Let 2 exist.");
            Assert.IsTrue(Engine.Exists("2"));
            Parser.Run("Forget 2.");
            Assert.IsFalse(Engine.Exists("2"));

            Parser.Run("Let the square root of 2 be 5.");
            Assert.AreEqual(5, Engine.GetValue("the square root of 2"));
            Parser.Run("Forget the square root of 2.");
            Assert.IsFalse(Engine.Exists("the square root of 2"));
        }

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
            Assert.AreEqual((decimal)(6+2.0/3.0), evaluator.EvaluateExpression("2 / .3"));
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
        }
    }
}
