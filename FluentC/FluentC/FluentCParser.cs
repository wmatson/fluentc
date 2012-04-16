using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentCEngine;
using System.IO;
using System.Text.RegularExpressions;
using FluentCEngine.Helpers;
using FluentCEngine.Constructs;

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
        private const string VARIABLE_WITHIN_STATEMENT = "(?<!\")\\b([^,.;?\"+/*-]+)\\b(?!\")";
        private const string PARENTHESIZED_NUMERICAL_EXPRESSION = "\\((-?\\d*\\.?\\d+(?: [-+/*] -?\\d*\\.?\\d+)*)\\)";
        private const string EXPRESSION_TYPE_SPLITTER = "(?:(\\(?-?\\d*\\.?\\d+(?:\\)? [-+/*] \\(?-?\\d*\\.?\\d+)*\\)?)|(?:\"(.+?)\")| \\+ )";
        private const int NUMERICAL_EXPRESSION_GROUP = 1;
        private const int STRING_EXPRESSION_GROUP = 2;
        private const int OPERATOR_GROUP = 2;
        private const int FIRST_OPERAND_GROUP = 1;
        private const int SECOND_OPERAND_GROUP = 3;

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
            Engine.DeclareVoidFunction("Tell me", new NativeVoidFunction( x => Console.WriteLine(x), new Parameter("message", VarType.String)));
        }

        /// <summary>
        /// Runs the script contained within the given string
        /// </summary>
        /// <param name="script">the script to run</param>
        public void Run(string script)
        {
            var matches = Regex.Matches(script, "\\b[^?]*?\\.");
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
            var result = Regex.Replace(SubstituteVariables(expression), EXPRESSION_TYPE_SPLITTER, e =>
            {
                if (!string.IsNullOrWhiteSpace(e.Groups[NUMERICAL_EXPRESSION_GROUP].Value))
                    return EvaluateNumericalExpression(e.Groups[NUMERICAL_EXPRESSION_GROUP].Value).ToString();
                else if (!string.IsNullOrWhiteSpace(e.Groups[STRING_EXPRESSION_GROUP].Value))
                    return Regex.Replace(e.Groups[STRING_EXPRESSION_GROUP].Value, "\"(.*?)\"", e2 => e2.Groups[1].Value);
                else
                    return "";
            });
            if(result.IsNumber()) 
                return decimal.Parse(result);
            return result;
        }

        private string SubstituteVariables(string expression)
        {
            var result = Regex.Replace(expression, VARIABLE_WITHIN_STATEMENT, e =>
            {
                var possibleVar = e.Groups[1].Value;
                if (!(possibleVar.IsNumber() || string.IsNullOrWhiteSpace(possibleVar)))
                {
                    var actualVar = Engine.Get(possibleVar);
                    if (actualVar.IsString)
                        possibleVar = string.Format("\"{0}\"", actualVar.Data);
                    else
                        possibleVar = actualVar.Data.ToString();
                }
                return possibleVar;
            });
            return result;
        }

        private decimal EvaluateNumericalExpression(string expression)
        {
            expression = Regex.Replace(expression, PARENTHESIZED_NUMERICAL_EXPRESSION, e => EvaluateNumericalExpression(e.Groups[1].Value).ToString());
            while (!Regex.IsMatch(expression, "^-?\\d*\\.?\\d+$"))
            {
                var firstOperationMatch = Regex.Match(expression, "(-?\\d*\\.?\\d+) ([/*]) (-?\\d*\\.?\\d+)");
                if (firstOperationMatch != Match.Empty)
                {
                    expression = expression.Remove(firstOperationMatch.Index, firstOperationMatch.Length);
                    var firstOperand = decimal.Parse(firstOperationMatch.Groups[FIRST_OPERAND_GROUP].Value);
                    var secondOperand = decimal.Parse(firstOperationMatch.Groups[SECOND_OPERAND_GROUP].Value);
                    if (firstOperationMatch.Groups[OPERATOR_GROUP].Value == "/")
                    {
                        expression = expression.Insert(firstOperationMatch.Index, (firstOperand / secondOperand).ToString());
                    }
                    else
                    {
                        expression = expression.Insert(firstOperationMatch.Index, (firstOperand * secondOperand).ToString());
                    }
                }
                else
                {
                    var secondOperationMatch = Regex.Match(expression, "(-?\\d*\\.?\\d+) ([-+]) (-?\\d*\\.?\\d+)");
                    expression = expression.Remove(secondOperationMatch.Index, secondOperationMatch.Length);
                    var firstOperand = decimal.Parse(secondOperationMatch.Groups[FIRST_OPERAND_GROUP].Value);
                    var secondOperand = decimal.Parse(secondOperationMatch.Groups[SECOND_OPERAND_GROUP].Value);
                    if (secondOperationMatch.Groups[OPERATOR_GROUP].Value == "-")
                    {
                        expression = expression.Insert(firstOperationMatch.Index, (firstOperand - secondOperand).ToString());
                    }
                    else
                    {
                        expression = expression.Insert(firstOperationMatch.Index, (firstOperand + secondOperand).ToString());
                    }
                }
            }
            return decimal.Parse(expression);
        }
    }
}
