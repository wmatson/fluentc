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
        private const string STATEMENT_GROUPING = "(.+?) (to(?: know)? )?\\b([^,.;?\"+/*-]+?)\\b(?: (be|exist|with) ?(.*))?((?:: .*)?\\.)";
        private const string DECLARATION_KEYWORD = "exist";
        private const string ASSIGNMENT_KEYWORD = "be";
        private const string MODIFICATION_KEYWORD = "Let";
        private const string DELETION_KEYWORD = "Forget";
        private const string FUNCTION_DECLARATION_KEYWORD = "How";
        private const string VOID_FUNCTION_FLAG = "to:";
        private const int KEYWORD_GROUP = 1;
        private const int FUNCTION_DECLARATION_FLAG_GROUP = 2;
        private const int ASSIGNMENT_VARIABLE_GROUP = 3;
        private const int VARIABLE_DECLARATION_FLAG_GROUP = 4;
        private const int ASSIGNMENT_EXPRESSION_GROUP = 5;
        private const int DECLARATION_PARAMETER_GROUP = 5;
        private const int SCRIPT_PART_GROUP = 6;
        private const string VARIABLE_WITHIN_STATEMENT = "(?<!\")\\b([^,.;?\"+/*-]+)\\b(?!\")";
        private const string PARENTHESIZED_NUMERICAL_EXPRESSION = "\\((-?\\d*\\.?\\d+(?: [-+/*] -?\\d*\\.?\\d+)*)\\)";
        private const string EXPRESSION_TYPE_SPLITTER = "(?:(\\(?-?\\d*\\.?\\d+(?:\\)? [-+/*] \\(?-?\\d*\\.?\\d+)*\\)?)|(?:\"(.+?)\")| \\+ )";
        private const int NUMERICAL_EXPRESSION_GROUP = 1;
        private const int STRING_EXPRESSION_GROUP = 2;
        private const int OPERATOR_GROUP = 2;
        private const int FIRST_OPERAND_GROUP = 1;
        private const int SECOND_OPERAND_GROUP = 3;

        private Engine PrimaryEngine { get { return Contexts.First(); } }
        private IEnumerable<Engine> Contexts { get; set; }

        /// <summary>
        /// Creates a new FluentCParser running on a fresh instance of Engine
        /// </summary>
        public FluentCParser() : this(new Engine()) { }

        /// <summary>
        /// Creates a new FluentCParser that runs on the given Engine
        /// </summary>
        /// <param name="engine">The engine on which to run</param>
        /// <param name="secondaryContexts">The engines to check for functions/variables if they are missing in the primary engine, these engines are evaluated in the order entered as parameters</param>
        public FluentCParser(params Engine[] contexts)
        {
            Contexts = contexts;
            PrimaryEngine.DeclareVoidFunction("Tell me", new NativeVoidFunction( x => Console.WriteLine(x), new ParameterMetaData("message")));
            
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
                    if (match.Groups[VARIABLE_DECLARATION_FLAG_GROUP].Value == ASSIGNMENT_KEYWORD)
                    {
                        var variableContext = GetVariableContext(match.Groups[ASSIGNMENT_VARIABLE_GROUP].Value);
                        variableContext.Assign(match.Groups[ASSIGNMENT_VARIABLE_GROUP].Value, EvaluateExpression(match.Groups[ASSIGNMENT_EXPRESSION_GROUP].Value, Contexts.ToArray()));
                    }
                    else if (match.Groups[VARIABLE_DECLARATION_FLAG_GROUP].Value == DECLARATION_KEYWORD)
                    {
                        PrimaryEngine.Declare(match.Groups[ASSIGNMENT_VARIABLE_GROUP].Value);
                    }
                    break;
                case DELETION_KEYWORD:
                    var varContext = GetVariableContext(match.Groups[ASSIGNMENT_VARIABLE_GROUP].Value);
                    varContext.Delete(match.Groups[ASSIGNMENT_VARIABLE_GROUP].Value);
                    break;
                case FUNCTION_DECLARATION_KEYWORD:
                    IEnumerable<ParameterMetaData> parameters = match.Groups[DECLARATION_PARAMETER_GROUP].Value.Split(',').Select(s => s.Trim()).Select(s => new ParameterMetaData(s));
                    PrimaryEngine.DeclareVoidFunction(match.Groups[ASSIGNMENT_VARIABLE_GROUP].Value,new FluentCVoidFunction(match.Groups[SCRIPT_PART_GROUP].Value,PrimaryEngine, parameters.ToArray()));
                    break;
                default://function invocation
                    ParseFunctionInvocation(statement);
                    break;
            }
        }

        private void ParseFunctionInvocation(string statement)
        {
            var functionName = statement.Split(',', '.')[0];
            while (!string.IsNullOrWhiteSpace(functionName) && !PrimaryEngine.VoidFunctionExists(functionName))
            {
                functionName = functionName.Substring(functionName.LastIndexOf(' '));
            }
            if (!string.IsNullOrWhiteSpace(functionName))
            {
                PrimaryEngine.RunVoidFunction(functionName);
            }
        }

        private Engine GetVariableContext(string variableName)
        {
            var variableContext = Contexts.FirstOrDefault(c => c.Exists(variableName));
            if (variableContext == null)
                variableContext = PrimaryEngine;
            return variableContext;
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

        private dynamic EvaluateExpression(string expression, params Engine[] contextQueue)
        {
            foreach (var context in contextQueue)
            {
                expression = SubstituteVariables(expression, context);
            }
            return EvaluateExpression(expression);
        }

        private string SubstituteVariables(string expression)
        {
            return SubstituteVariables(expression, PrimaryEngine);
        }

        private string SubstituteVariables(string expression, Engine context)
        {
            var result = Regex.Replace(expression, VARIABLE_WITHIN_STATEMENT, e =>
            {
                var possibleVar = e.Groups[1].Value;
                if (!(possibleVar.IsNumber() || string.IsNullOrWhiteSpace(possibleVar)) && context.Exists(possibleVar))
                {
                    var actualVar = context.Get(possibleVar);
                    if (actualVar.IsString)
                        possibleVar = string.Format("\"{0}\"", actualVar.Data);
                    else
                        possibleVar = actualVar.Data.ToString();
                }
                return possibleVar;
            });
            return result;
        }

        #region Numerical Expressions
        private static decimal EvaluateNumericalExpression(string expression)
        {
            while (Regex.IsMatch(expression, PARENTHESIZED_NUMERICAL_EXPRESSION))
            {
                expression = Regex.Replace(expression, PARENTHESIZED_NUMERICAL_EXPRESSION, e => EvaluateNumericalExpression(e.Groups[1].Value).ToString());
            }
            expression = EvaluateMultiplicationAndDivision(expression);
            expression = EvaluateAdditionAndSubtraction(expression);
            return decimal.Parse(expression);
        }


        private static string EvaluateAdditionAndSubtraction(string expression)
        {
            while (Regex.IsMatch(expression, " [-+] "))
            {
                var secondOperationMatch = Regex.Match(expression, "(-?\\d*\\.?\\d+) ([-+]) (-?\\d*\\.?\\d+)");
                expression = expression.Remove(secondOperationMatch.Index, secondOperationMatch.Length);
                var firstOperand = decimal.Parse(secondOperationMatch.Groups[FIRST_OPERAND_GROUP].Value);
                var secondOperand = decimal.Parse(secondOperationMatch.Groups[SECOND_OPERAND_GROUP].Value);
                if (secondOperationMatch.Groups[OPERATOR_GROUP].Value == "-")
                {
                    expression = expression.Insert(secondOperationMatch.Index, (firstOperand - secondOperand).ToString());
                }
                else
                {
                    expression = expression.Insert(secondOperationMatch.Index, (firstOperand + secondOperand).ToString());
                }
            }
            return expression;
        }


        private static string EvaluateMultiplicationAndDivision(string expression)
        {
            while (Regex.IsMatch(expression, " [*/] "))
            {
                var firstOperationMatch = Regex.Match(expression, "(-?\\d*\\.?\\d+) ([/*]) (-?\\d*\\.?\\d+)");
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
            return expression;
        }
        #endregion
    }
}
