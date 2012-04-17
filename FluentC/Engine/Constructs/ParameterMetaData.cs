using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FluentCEngine.Constructs
{
    public class ParameterMetaData
    {
        public ParameterMetaData(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }
    }
}
