using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentCEngine.Interfaces;

namespace FluentCEngine.Constructs
{
    public class NativeValuedFunction : ValuedFunction
    {
        private Func<object[], dynamic> Function {get;set;}

        public NativeValuedFunction(Func<object[], dynamic> function, params ParameterMetaData[] parameters)
        {
            Function = function;
            Parameters = parameters;
        }

        public dynamic Run(params object[] parameters)
        {
            return Function(parameters);
        }

        public IEnumerable<ParameterMetaData> Parameters { get; private set; }
    }
}
