using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentCEngine;
using FluentC;
using FluentCEngine.Constructs;

namespace Tests
{
    [TestClass]
    public class ValuedFunctions
    {

        private Engine Engine { get; set; }
        private FluentCParser Parser { get; set; }


        [TestInitialize]
        public void Initialize()
        {
            Engine = new Engine();
            Parser = new FluentCParser(Engine);
        }

        [TestMethod]
        public void TestMethodDeclaration()
        {
            Assert.IsFalse(Engine.FunctionExists("doSomething"));
            Engine.DeclareValuedFunction("doSomething", new NativeValuedFunction(x => x.ToString(), new ParameterMetaData("item")));
            Assert.IsTrue(Engine.FunctionExists("doSomething"));
            Assert.IsFalse(Engine.FunctionExists("FluentC Valued Function"));
            Engine.DeclareValuedFunction("FluentC Valued Function", new FluentCValuedFunction("Let x be \"Hello World.\".","x!", Engine));
            Assert.IsTrue(Engine.FunctionExists("FluentC Valued Function"));
        }

        [TestMethod]
        public void TestNativeFunctionExistence()
        {
            Assert.IsTrue(Engine.FunctionExists("Ask me for a number"));
            Assert.IsTrue(Engine.FunctionExists("Ask me for a string"));
            Assert.IsTrue(Engine.FunctionExists("Give me the length of"));
            Assert.IsTrue(Engine.FunctionExists("Give me the part of"));
        }

        [TestMethod]
        public void TestScriptMethodDeclaration()
        {
            Assert.IsFalse(Engine.FunctionExists("the square of"));
            Parser.Run("How to know the square of with x: Let x be x * x. x!");
            Assert.IsTrue(Engine.FunctionExists("the square of"));

            Parser.Run("How to know skydive with turtles: Let five be 5; Let two be 2; Let seven be five + two. five!");
            Assert.IsTrue(Engine.FunctionExists("skydive"));
            Assert.IsFalse(Engine.Exists("five"));
            Assert.IsFalse(Engine.Exists("two"));
            Assert.IsFalse(Engine.Exists("five"));
            Assert.IsFalse(Engine.Exists("seven"));

            Parser.Run("How to know how to do something that uses multiple words in its name: Let my favorite var be 0; Let another variable be \"Hello\". my favorite var! Let one exist.");
            Assert.IsFalse(Engine.Exists("my favorite var"));
            Assert.IsFalse(Engine.Exists("another variable"));
            Assert.IsTrue(Engine.Exists("one"));
            Assert.IsTrue(Engine.FunctionExists("how to do something that uses multiple words in its name"));
            Parser.Run("How to know stuff with stuff: Let x be stuff. stuff!");
            Assert.IsTrue(Engine.FunctionExists("stuff"));
            Assert.IsFalse(Engine.Exists("stuff"));
        }

        [TestMethod]
        public void TestScriptFunctionInvocation()
        {
            Parser.Run("Let x be 0. How to know increment and give me the value: Let x be x + 1. x!");
            Assert.AreEqual(0, Engine.GetValue("x"));
            Assert.AreEqual(1, Parser.EvaluateExpression("increment and give me the value"));

            Assert.IsFalse(Engine.Exists("y"));
            Parser.Run("How to know the square of with y: Let y be y * y. y!");
            Assert.IsFalse(Engine.Exists("y"));
            Assert.AreEqual(16, Parser.EvaluateExpression("the square of 4"));
            Assert.IsFalse(Engine.Exists("y"));
        }
    }
}
