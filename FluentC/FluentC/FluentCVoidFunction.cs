using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentCEngine.Interfaces;
using FluentCEngine.Constructs;
using FluentCEngine;

namespace FluentC
{
    public class FluentCVoidFunction : VoidFunction
    {
        private Engine GlobalContext { get; set; }
        private string Script { get; set; }

        public FluentCVoidFunction(string script, Engine globalContext, params ParameterMetaData[] parameters)
        {
            Parameters = parameters;
            GlobalContext = globalContext;
            Script = script;
        }

        public void Run(params object[] parameters)
        {
            var localContext = new Engine();
            var parameterNames = Parameters.Select(p => p.Name).ToArray();
            for (int i = 0; i < parameters.Length; i++)
            {
                localContext.Assign(parameterNames[i], parameters[i]);
            }
            FluentCParser parser = new FluentCParser(localContext, GlobalContext);
            parser.RunBlock(Script);
        }

        public IEnumerable<ParameterMetaData> Parameters { get; private set; }
    }
}
