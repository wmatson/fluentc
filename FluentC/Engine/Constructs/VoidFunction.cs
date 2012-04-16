using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FluentCEngine.Constructs
{
    public abstract class VoidFunction
    {

        public abstract bool IsNative { get; }

        //Native methods do not have a defined return string.
        public abstract string RunAsScript(params object[] parameters);

        public abstract IEnumerable<Parameter> Parameters { get; }

    }
}
