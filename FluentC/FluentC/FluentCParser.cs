using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentCEngine;
using System.IO;
using System.Text.RegularExpressions;
using FluentCEngine.Helpers;
using FluentCEngine.Constructs;
using System.Speech.Synthesis;

namespace FluentC
{
    /// <summary>
    /// Contains methods with which to run scripts written in accordance with the FluentC specification
    /// </summary>
    public class FluentCParser
    {
        #region regex constants
        private const string STATEMENT_GROUPING = "(.+?) (?:(to(?: know)? )?|(?:([^,]+), )?)\\b([^,\\.;?\"+/*-]+?)\\b(?: (be|exist|with) ?([^:]*))?((?:: [^\\.]*)?[\\.;])([^\\.]*?!)?";
        private const string DECLARATION_KEYWORD = "exist";
        private const string ASSIGNMENT_KEYWORD = "be";
        private const string MODIFICATION_KEYWORD = "Let";
        private const string DELETION_KEYWORD = "Forget";
        private const string FUNCTION_DECLARATION_KEYWORD = "How";
        private const string VOID_FUNCTION_FLAG = "to ";
        private const string VALUED_FUNCTION_FLAG = "to know ";
        private const int KEYWORD_GROUP = 1;
        private const int FUNCTION_DECLARATION_FLAG_GROUP = 2;
        private const int ASSIGNMENT_VARIABLE_GROUP = 4;
        private const int VARIABLE_DECLARATION_FLAG_GROUP = 5;
        private const int ASSIGNMENT_EXPRESSION_GROUP = 6;
        private const int DECLARATION_PARAMETER_GROUP = 6;
        private const int SCRIPT_PART_GROUP = 7;
        private const int RETURN_EXPRESSION_GROUP = 8;
        private const string VARIABLE_WITHIN_STATEMENT = "(?<!\")\\b([^,.;?\"+/*-]+)\\b(?!\")";
        private const string PARENTHESIZED_NUMERICAL_EXPRESSION = "\\((-?\\d*\\.?\\d+(?: [-+/*] -?\\d*\\.?\\d+)*)\\)";
        private const string PARENTHESIZED_EXPRESSION = "\\(([^()]*)\\)";
        private const string EXPRESSION_TYPE_SPLITTER = "(?:(-?\\d*\\.?\\d+ (?:is larger than|is smaller than|is the same as|[><=]) -?\\d*\\.?\\d+)|(-?\\d*\\.?\\d+(?: [-+/*] -?\\d*\\.?\\d+(?! [><=]))*)|(?:\"(.+?)\")| \\+ |(?:\\b([^\\.;]+)))";
        private const int CONDITIONAL_EXPRESSION_GROUP = 1;
        private const int NUMERICAL_EXPRESSION_GROUP = 2;
        private const int STRING_EXPRESSION_GROUP = 3;
        private const int VALUED_FUNCTION_EXPRESSION_GROUP = 4;
        private const int OPERATOR_GROUP = 2;
        private const int FIRST_OPERAND_GROUP = 1;
        private const int SECOND_OPERAND_GROUP = 3;
        #endregion



        private Engine PrimaryEngine { get { return Contexts.First(); } }
        private IEnumerable<Engine> Contexts { get; set; }

        private SpeechSynthesizer Synth {get;set;}

        private bool _speakOnTellMe = false;
        public bool SpeakOnTellMe
        {
            get { return _speakOnTellMe; }
            set
            {
                if (value)
                {
                    Synth = new SpeechSynthesizer();
                    Synth.SelectVoice(Synth.GetInstalledVoices().First().VoiceInfo.Name);
                    Synth.Volume = 100;
                }
                _speakOnTellMe = value;
            }
        }

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
            
            PrimaryEngine.DeclareVoidFunction("Tell me", new NativeVoidFunction( x => {
                Console.WriteLine(x[0]);
                if (SpeakOnTellMe)
                {
                    Synth.Speak(x[0].ToString());
                }
            }, new ParameterMetaData("message")));
            PrimaryEngine.DeclareValuedFunction("Ask me for a string", new NativeValuedFunction(prompt =>
                {
                    Console.WriteLine(prompt[0].ToString());
                    Console.Write(">");
                    return Console.ReadLine();
                }, new ParameterMetaData("Prompt")));
            PrimaryEngine.DeclareValuedFunction("Ask me for a number", new NativeValuedFunction(prompt => 
            {
                string entry = "";
                while (!entry.IsNumber())
                {
                    Console.WriteLine(prompt[0].ToString());
                    Console.Write("(Number) >");
                    entry = Console.ReadLine();
                    if(!entry.IsNumber())
                        Console.WriteLine("Please enter a number.");
                }
                return entry;
            }, new ParameterMetaData("Prompt")));
            PrimaryEngine.DeclareValuedFunction("Give me the part of", new NativeValuedFunction( parameters =>
            {
                return parameters[0].ToString().Substring(int.Parse(parameters[1].ToString()), int.Parse(parameters[2].ToString()));
            }, new ParameterMetaData("string"), new ParameterMetaData("starting index"), new ParameterMetaData("length")));
            PrimaryEngine.DeclareValuedFunction("Give me the length of", new NativeValuedFunction(parameters =>
            {
                return parameters[0].ToString().Length;
            }, new ParameterMetaData("string")));
            
        }

        /// <summary>
        /// Runs the script contained within the given string
        /// </summary>
        /// <param name="script">the script to run</param>
        public void Run(string script)
        {
            var matches = Regex.Matches(script, "\\b[^?]*?[\\.]([^\\.]*?!)?(\\s+|$)");
            for (int i = 0; i < matches.Count; i++ )
            {
                var match = matches[i];
                ParseStatement(match.Value);
            }
        }

        /// <summary>
        /// Runs the script contained within the given string
        /// </summary>
        /// <param name="script">the script to run</param>
        public void RunBlock(string script)
        {
            var matches = Regex.Matches(script, "\\b[^?]*?[\\.;]");
            for (int i = 0; i < matches.Count; i++)
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
                        variableContext.Assign(match.Groups[ASSIGNMENT_VARIABLE_GROUP].Value, EvaluateExpression(match.Groups[ASSIGNMENT_EXPRESSION_GROUP].Value));
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
                    var functionName = match.Groups[ASSIGNMENT_VARIABLE_GROUP].Value;
                    var script = match.Groups[SCRIPT_PART_GROUP].Value;
                    if(match.Groups[FUNCTION_DECLARATION_FLAG_GROUP].Value == VOID_FUNCTION_FLAG)
                        PrimaryEngine.DeclareVoidFunction(functionName,new FluentCVoidFunction(script,PrimaryEngine, parameters.ToArray()));
                    else
                        PrimaryEngine.DeclareValuedFunction(functionName, new FluentCValuedFunction(script, match.Groups[RETURN_EXPRESSION_GROUP].Value, PrimaryEngine, parameters.ToArray()));
                    break;
                default://function invocation
                    ParseFunctionInvocation(statement);
                    break;
            }
        }

        private dynamic ParseFunctionInvocation(string statement)
        {
            var functionName = statement.Split(',', '.', ';')[0];
            while (!string.IsNullOrWhiteSpace(functionName) && !Contexts.Any( e => e.FunctionExists(functionName)))
            {
                functionName = functionName.Substring(0, functionName.LastIndexOf(' '));
            }
            if (!string.IsNullOrWhiteSpace(functionName))
            {
                var parameterString = Regex.Match(statement, string.Format("(?:.{{{0}}})(?: with )?(.*)[\\.;]?",functionName.Length)).Groups[1].Value;
                var parameters = parameterString.Split(',')
                        .Where(s => !string.IsNullOrWhiteSpace(s) && s != ".")
                        .Select( s=> EvaluateExpression(s, Contexts.ToArray()))
                        .ToArray();
                var functionContext = Contexts.First(e => e.FunctionExists(functionName));
                return functionContext.RunFunction(functionName, parameters);
            }
            return null;
        }

        private Engine GetVariableContext(string variableName)
        {
            var variableContext = Contexts.FirstOrDefault(c => c.Exists(variableName));
            if (variableContext == null)
                variableContext = PrimaryEngine;
            return variableContext;
        }

        #region expression evaluation

        /// <summary>
        /// Evaluates the given expression using the Contexts contained within this FluentCParser.
        /// </summary>
        /// <param name="expression">The expression to evaluate</param>
        /// <returns>the result of evaluating the given expression over the internal contexts of this FluentCParser</returns>
        public dynamic EvaluateExpression(string expression)
        {
            return EvaluateExpression(expression, Contexts.ToArray());
        }

        private dynamic EvaluateExpression(string expression, params Engine[] contextQueue)
        {
            foreach (var context in contextQueue)
            {
                expression = SubstituteVariables(expression, context);
            }
            return EvaluateRawExpression(expression);
        }

        /// <summary>
        /// Evaluates the given expression using the Contexts contained within this FluentCParser.
        /// </summary>
        /// <param name="expression">The expression to evaluate</param>
        /// <returns>the result of evaluating the given expression over the internal contexts of this FluentCParser</returns>
        public dynamic EvaluateRawExpression(string expression)
        {
            while (Regex.IsMatch(expression, PARENTHESIZED_EXPRESSION))
            {
                expression = Regex.Replace(expression, PARENTHESIZED_NUMERICAL_EXPRESSION, e => EvaluateRawExpression(e.Groups[1].Value).ToString());
            }
            var result = Regex.Replace(expression, EXPRESSION_TYPE_SPLITTER, e =>
            {
                if (!string.IsNullOrWhiteSpace(e.Groups[NUMERICAL_EXPRESSION_GROUP].Value))
                    return EvaluateNumericalExpression(e.Groups[NUMERICAL_EXPRESSION_GROUP].Value).ToString();
                else if (!string.IsNullOrWhiteSpace(e.Groups[STRING_EXPRESSION_GROUP].Value))
                    return Regex.Replace(e.Groups[STRING_EXPRESSION_GROUP].Value, "\"(.*?)\"", e2 => e2.Groups[1].Value);
                else if (!string.IsNullOrWhiteSpace(e.Groups[VALUED_FUNCTION_EXPRESSION_GROUP].Value))
                    return ParseFunctionInvocation(e.Groups[VALUED_FUNCTION_EXPRESSION_GROUP].Value).ToString();
                else if (!string.IsNullOrWhiteSpace(e.Groups[CONDITIONAL_EXPRESSION_GROUP].Value))
                    return false.ToString();
                else
                    return "";
            });
            if (result.IsNumber())
                return decimal.Parse(result);
            if (result.IsCondition())
                return bool.Parse(result);
            return result;
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
        #endregion
    }
}
