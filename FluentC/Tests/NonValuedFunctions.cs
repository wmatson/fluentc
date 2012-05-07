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
            Assert.IsFalse(Engine.FunctionExists("doSomething"));
            Engine.DeclareVoidFunction("doSomething", new NativeVoidFunction(x => x.ToString(), new ParameterMetaData("item")));
            Assert.IsTrue(Engine.FunctionExists("doSomething"));
            Assert.IsFalse(Engine.FunctionExists("FluentC Void Function"));
            Engine.DeclareVoidFunction("FluentC Void Function", new FluentCVoidFunction("Tell me \"Hello World.\".", Engine));
            Assert.IsTrue(Engine.FunctionExists("FluentC Void Function"));
        }

        [TestMethod]
        public void TestNativeFunctionExistence()
        {
            Engine.FunctionExists("Tell me");
        }

        [TestMethod]
        public void TestScriptMethodDeclaration()
        {
            Assert.IsFalse(Engine.FunctionExists("add"));
            Parser.Run("How to add with x, y: Let z be x + y.");
            Assert.IsTrue(Engine.FunctionExists("add"));
            Parser.Run("Let x be 0. How to increment: Let x be x + 1.");
            Assert.IsTrue(Engine.FunctionExists("increment"));
            Parser.Run("How to skydive with turtles: Let five be 5; Let two be 2; Let seven be five + two.");
            Assert.IsTrue(Engine.FunctionExists("skydive"));
            Assert.IsFalse(Engine.Exists("five"));
            Assert.IsFalse(Engine.Exists("two"));
            Assert.IsFalse(Engine.Exists("five"));
            Assert.IsFalse(Engine.Exists("seven"));
            Parser.Run("How to do something that uses multiple words in its name: Let my favorite var be 0; Let another variable be \"Hello\". Let one exist.");
            Assert.IsFalse(Engine.Exists("my favorite var"));
            Assert.IsFalse(Engine.Exists("another variable"));
            Assert.IsTrue(Engine.Exists("one"));
            Assert.IsTrue(Engine.FunctionExists("do something that uses multiple words in its name"));
            Parser.Run("How to stuff with stuff: Let x be stuff.");
            Assert.IsTrue(Engine.FunctionExists("stuff"));
            Assert.IsFalse(Engine.Exists("stuff"));
        }

        [TestMethod]
        public void TestScriptFunctionInvocation()
        {
            Parser.Run("Let x be 0. How to increment: Let x be x + 1.");
            Assert.AreEqual(0, Engine.GetValue("x"));
            Parser.Run("increment.");
            Assert.AreEqual(1, Engine.GetValue("x"));

            Assert.IsFalse(Engine.Exists("y"));
            Parser.Run("How to decrement with y: Let y be y - 1.");
            Assert.IsFalse(Engine.Exists("y"));
            Parser.Run("decrement x.");
            Assert.IsFalse(Engine.Exists("y"));
            Parser.Run("decrement with x.");
            Assert.IsFalse(Engine.Exists("y"));

            Assert.IsFalse(Engine.Exists("result"));
            Parser.Run("Let result exist. How to decrement and store in result with y: Let y be y - 1; Let result be y.");
            Assert.IsTrue(Engine.Exists("result"));
            Parser.Run("decrement and store in result 3.");
            Assert.AreEqual(2M, Engine.GetValue("result"));
            Parser.Run("decrement and store in result with 5.");
            Assert.AreEqual(4M, Engine.GetValue("result"));

            Parser.Run("How to add and store in result with x, y: Let result be x + y.");
            Assert.IsTrue(Engine.FunctionExists("add and store in result"));
            Parser.Run("add and store in result with 1.2, 6.");
            Assert.AreEqual(7.2M, Engine.GetValue("result"));

            Parser.Run("add and store in result with -1.2, 6.");
            Assert.AreEqual(4.8M, Engine.GetValue("result"));

            Parser.Run("Let randomVar be 5. How to do stuff: Let randomVar be 5 + randomVar; Let randomVar be 3.4.");
            Parser.Run("do stuff.");
            Assert.AreEqual(3.4M, Engine.GetValue("randomVar"));

            Parser.Run("Let randomVar be 5. How to do other stuff: Let randomVar be 5 + randomVar; Let randomVar be 3.4 + \" Hello\".");
            Parser.Run("do other stuff.");
            Assert.AreEqual("3.4 Hello", Engine.GetValue("randomVar"));

            Parser.Run("Let randomVar be 5. How to do concatenation stuff: Let randomVar be 5 + randomVar; Let randomVar be \"something with: in it \" + 3.4 + \" Hello\".");
            Parser.Run("do concatenation stuff.");
            Assert.AreEqual("something with: in it 3.4 Hello", Engine.GetValue("randomVar"));

            Parser.Run("Let randomVar be 5. How to do other concatenation stuff: Let randomVar be 5 + randomVar; Let randomVar be \"something with . in it \" + 3.4 + \" Hello\".");
            Parser.Run("do other concatenation stuff.");
            Assert.AreEqual("something with . in it 3.4 Hello", Engine.GetValue("randomVar"));
        }
    }
}
