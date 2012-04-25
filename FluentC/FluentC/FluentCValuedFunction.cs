using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentCEngine.Interfaces;
using FluentCEngine;
using FluentCEngine.Constructs;

namespace FluentC
{
    public class FluentCValuedFunction : ValuedFunction
    {
        private Engine GlobalContext { get; set; }
        private string Script { get; set; }
        private string ReturnExpression { get; set; }

        public FluentCValuedFunction(string script, string returnExpression, Engine globalContext, params ParameterMetaData[] parameters)
        {
            Parameters = parameters;
            GlobalContext = globalContext;
            Script = script;
            ReturnExpression = returnExpression.Substring(0, returnExpression.IndexOf('!'));
        }

        public dynamic Run(params object[] parameters)
        {
            var localContext = new Engine();
            var parameterNames = Parameters.Select(p => p.Name).ToArray();
            for (int i = 0; i < parameters.Length; i++)
            {
                localContext.Assign(parameterNames[i], parameters[i]);
            }
            FluentCParser parser = new FluentCParser(localContext, GlobalContext);
            parser.RunBlock(Script);
            return parser.EvaluateExpression(ReturnExpression);
        }

        public IEnumerable<ParameterMetaData> Parameters { get; private set; }
    }
}
