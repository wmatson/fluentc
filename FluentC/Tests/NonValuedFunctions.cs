using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentCEngine;
using FluentC;

namespace Tests
{
    [TestClass]
    public class NonValuedFunctions
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
        public void TestCommenting()
        {
            Parser.Run("First comment? Let x be 2. Second comment? Let y be 4. Let x be x + y.");
            Assert.AreEqual(6M, Engine.GetValue("x"));
            Parser.Run("Let y be x + y. Comment 3? Let x be 5? Let y be x + y.");
            Assert.AreEqual(16M, Engine.GetValue("y"));
            Parser.Run("? //? ?");
        }

        [TestMethod]
        public void TestMethodDeclaration()
        {
            Assert.Fail("Unwritten.");
        }

        [TestMethod]
        public void TestNativeFunctionExistence()
        {
            Engine.VoidFunctionExists("Tell me");
        }

        [TestMethod]
        public void TestScriptMethodDeclaration()
        {
            Parser.Run("How to add with x, y: Let z be x + y.");
            Assert.IsTrue(Engine.VoidFunctionExists("add"));
            Parser.Run("Let x be 0. How to increment: Let x be x + 1.");
            Assert.IsTrue(Engine.VoidFunctionExists("increment"));
            Parser.Run("How to skydive with turtles: Let five be 5; Let two be 2; Let seven be five + two.");
            Assert.IsTrue(Engine.VoidFunctionExists("skydive"));
            Assert.IsFalse(Engine.Exists("five"));
            Assert.IsFalse(Engine.Exists("two"));
            Assert.IsFalse(Engine.Exists("five"));
            Assert.IsFalse(Engine.Exists("seven"));
            Parser.Run("How to do something that uses multiple words in its name: Let my favorite var be 0; Let another variable be \"Hello\". Let one exist.");
            Assert.IsFalse(Engine.Exists("my favorite var"));
            Assert.IsFalse(Engine.Exists("another variable"));
            Assert.IsTrue(Engine.Exists("one"));
            Assert.IsTrue(Engine.VoidFunctionExists("do something that uses multiple words in its name"));
            Parser.Run("How to stuff with stuff: Let x be stuff.");
            Assert.IsTrue(Engine.VoidFunctionExists("stuff"));
            Assert.IsFalse(Engine.Exists("stuff"));
        }

        [TestMethod]
        public void TestScriptFunctionInvocation()
        {
            Parser.Run("Let x be 0. How to increment: Let x be x + 1.");
            Assert.AreEqual(0, Engine.GetValue("x"));
            Parser.Run("increment.");
            Assert.AreEqual(1, Engine.GetValue("x"));
        }
    }
}
