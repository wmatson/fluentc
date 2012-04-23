using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentCEngine.Constructs;

namespace FluentCEngine.Interfaces
{
    public interface ValuedFunction
    {
        dynamic Run(params object[] parameters);

        IEnumerable<ParameterMetaData> Parameters { get; }
    }
}
