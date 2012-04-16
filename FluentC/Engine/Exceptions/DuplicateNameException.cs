using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FluentCEngine.Exceptions
{
    public class DuplicateNameException : Exception
    {
        public string DuplicatedName { get; set; }

        public DuplicateNameException(string duplicatedName)
        {
            DuplicatedName = duplicatedName;
        }

        public override string Message
        {
            get
            {
                return string.Format("The name \"{0}\" has already been used in the current context.", DuplicatedName);
            }
        }
    }
}
