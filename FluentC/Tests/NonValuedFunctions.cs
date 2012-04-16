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
        }

        [TestMethod]
        public void TestMethodDeclaration()
        {

        }
    }
}
