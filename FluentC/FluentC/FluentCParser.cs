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
        private const string STATEMENT_GROUPING = "(.+?) \\b([^;,\\.?]+?)\\b(?: (be|exist) ?(.*))?\\.";
        private const string DECLARATION_KEYWORD = "exist";
        private const string ASSIGNMENT_KEYWORD = "be";
        private const string MODIFICATION_KEYWORD = "Let";
        private const string DELETION_KEYWORD = "Forget";
        private const int KEYWORD_GROUP = 1;
        private const int ASSIGNMENT_VARIABLE_GROUP = 2;
        private const int DECLARATION_FLAG_GROUP = 3;
        private const int ASSIGNMENT_EXPRESSION_GROUP = 4;

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
            var match = Regex.Match(statement, STATEMENT_GROUPING);
            switch (match.Groups[KEYWORD_GROUP].Value)
            {
                case MODIFICATION_KEYWORD:
                    if (match.Groups[DECLARATION_FLAG_GROUP].Value == ASSIGNMENT_KEYWORD)
                    {
                        Engine.Assign(match.Groups[ASSIGNMENT_VARIABLE_GROUP].Value, EvaluateExpression(match.Groups[ASSIGNMENT_EXPRESSION_GROUP].Value));
                    }
                    else if (match.Groups[DECLARATION_FLAG_GROUP].Value == DECLARATION_KEYWORD)
                    {
                        Engine.Declare(match.Groups[ASSIGNMENT_VARIABLE_GROUP].Value);
                    }
                    break;
                case DELETION_KEYWORD:
                    Engine.Delete(match.Groups[ASSIGNMENT_VARIABLE_GROUP].Value);
                    break;
            }
        }

        private dynamic EvaluateExpression(string expression)
        {
            //TODO make this method actually evaluate expressions
            var result = Regex.Replace(expression, "(?<!\")\\b([^,.;?\"+/*-]+)\\b(?!\")", e => {
                var possibleVar = e.Groups[1].Value;
                if(!(possibleVar.IsNumber() || string.IsNullOrWhiteSpace(possibleVar))) {
                    var actualVar = Engine.Get(possibleVar);
                    if (actualVar.IsString)
                        possibleVar = string.Format("\"{0}\"", actualVar.Data);
                    else
                        possibleVar = actualVar.Data.ToString();
                }
                return possibleVar;
            });
            result = Regex.Replace(result, "\"(.*)\"", e => e.Groups[1].Value);
            if(result.IsNumber()) 
                return decimal.Parse(result);
            return result;
        }
    }
}
