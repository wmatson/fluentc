﻿using System;
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
            Engine.FunctionExists("Ask me for a number");
            Engine.FunctionExists("Ask me for a string");
            Engine.FunctionExists("Give me the length of the string");
            Engine.FunctionExists("Give me the part of the string");
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

            Parser.Run("How to know how to do something that uses multiple words in its name: Let my favorite var be 0; Let another variable be \"Hello\". Let one exist. my favorite var!");
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

            //Assert.IsFalse(Engine.Exists("result"));
            //Parser.Run("Let result exist. How to decrement and store in result with y: Let y be y - 1; Let result be y.");
            //Assert.IsTrue(Engine.Exists("result"));
            //Parser.Run("decrement and store in result 3.");
            //Assert.AreEqual(2M, Engine.GetValue("result"));
            //Parser.Run("decrement and store in result with 5.");
            //Assert.AreEqual(4M, Engine.GetValue("result"));

            //Parser.Run("How to add and store in result with x, y: Let result be x + y.");
            //Assert.IsTrue(Engine.FunctionExists("add and store in result"));
            //Parser.Run("add and store in result with 1.2, 6.");
            //Assert.AreEqual(7.2M, Engine.GetValue("result"));

            //Parser.Run("add and store in result with -1.2, 6.");
            //Assert.AreEqual(4.8M, Engine.GetValue("result"));
        }
    }
}