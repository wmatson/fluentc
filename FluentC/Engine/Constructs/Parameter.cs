using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FluentCEngine.Constructs
{
    public class Parameter
    {
        public Parameter(string name, VarType type)
        {
            Name = name;
            Type = type;
        }

        public VarType Type { get; private set; }
        public string Name { get; private set; }
    }
}
