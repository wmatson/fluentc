using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FluentCEngine.Exceptions
{
    public class InvalidFunctionException : Exception
    {
        public string AttemptedStatement {get;set;}

        public InvalidFunctionException(string statement)
        {
            AttemptedStatement = statement;
        }

        public override string Message
        {
            get
            {
                return string.Format("The expression \"{0}\" was attempted to be parsed as a function and no function could be found.", AttemptedStatement);
            }
        }
    }
}
