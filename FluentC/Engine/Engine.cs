using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FluentCEngine
{
    public class Engine
    {
        private Dictionary<string, Variable> Variables { get; set; }

        public Engine()
        {
            Variables = new Dictionary<string, Variable>();
        }

        public bool Declare(string variable)
        {
            try
            {
                Variables.Add(variable, null);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public void Assign(string variable, Variable value)
        {
            if (!Exists(variable))
            {
                Declare(variable);
            }
            Variables[variable] = value;
        }

        public Variable Get(string variable)
        {
            if (Exists(variable))
            {
                return Variables[variable];
            }
            return null;
        }

        public bool Exists(string variable)
        {
            return Variables.Keys.Any(v => v == variable);
        }
    }
}
