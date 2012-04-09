using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FluentCEngine.Exceptions
{
    /// <summary>
    /// An exception to be thrown when there is an attempt to access a variable that does not yet exist
    /// </summary>
    public class UndeclaredVariableException : Exception
    {
        public string VariableName {get;set;}

        public UndeclaredVariableException(string variable)
        {
            VariableName = variable;
        }

        public override string Message
        {
            get
            {
                return string.Format("The variable with the name \"{0}\" does not exist");
            }
        }
    }
}
