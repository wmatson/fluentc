using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentCEngine;
using System.IO;
using System.Text.RegularExpressions;
using FluentCEngine.Helpers;

namespace FluentC
{
    /// <summary>
    /// Contains methods with which to run scripts written in accordance with the FluentC specification
    /// </summary>
    public class FluentCParser
    {
        private Engine Engine { get; set; }

        /// <summary>
        /// Creates a new FluentCParser running on a fresh instance of Engine
        /// </summary>
        public FluentCParser() : this(new Engine()) { }

        /// <summary>
        /// Creates a new FluentCParser that runs on the given Engine
        /// </summary>
        /// <param name="engine">The engine on which to run</param>
        public FluentCParser(Engine engine)
        {
            Engine = engine;
        }

        /// <summary>
        /// Runs the script contained within the given string
        /// </summary>
        /// <param name="script">the script to run</param>
        public void Run(string script)
        {
            var matches = Regex.Matches(script, "\\b.*?\\.");
            for (int i = 0; i < matches.Count; i++ )
            {
                var match = matches[i];
                ParseStatement(match.Value);
            }
        }

        /// <summary>
        /// Runs the script contained within the file denoted by the given filename
        /// </summary>
        /// <param name="filename">the name of the file to run as a script</param>
        public void RunFile(string filename)
        {
            Run(File.ReadAllText(filename));
        }

        private void ParseStatement(string statement)
        {
            var match = Regex.Match(statement, "(.+?) \\b([^;,\\.?]+?)\\b(?: (be|exist) ?(.*))?\\.");
            switch (match.Groups[1].Value)
            {
                case "Let":
                    if (match.Groups[3].Value == "be")
                    {
                        Console.WriteLine("Assign -- {0} -- {1}", match.Groups[2], match.Groups[4]);
                        Engine.Assign(match.Groups[2].Value, EvaluateExpression(match.Groups[4].Value));
                    }
                    else if (match.Groups[3].Value == "exist")
                    {
                        Console.WriteLine("Declare -- {0}", match.Groups[2]);
                        Engine.Declare(match.Groups[2].Value);
                    }
                    break;
                case "Forget":
                    Console.WriteLine("Delete -- {0}", match.Groups[2].Value);
                    Engine.Delete(match.Groups[2].Value);
                    break;
            }
        }

        private dynamic EvaluateExpression(string expression)
        {
            //TODO make this method actually evaluate expressions
            var result = Regex.Replace(expression, "\"(.*)\"", e => e.Groups[1].Value);
            if(result.IsNumber()) 
                return decimal.Parse(result);
            return result;
        }
    }
}
