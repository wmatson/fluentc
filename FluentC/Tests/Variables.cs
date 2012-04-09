﻿using System;
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
            Assert.Fail("Not implemented");
        }

        [TestMethod]
        public void TestScriptDeletion()
        {
            Assert.Fail("Not implemented");
        }
    }
}
