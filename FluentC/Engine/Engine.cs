using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FluentCEngine
{
    /// <summary>
    /// Encapsulates functionality for core features of simple scripting languages.
    /// 
    /// These features include variable management, conditional checking, function management, and loop execution.
    /// </summary>
    public class Engine
    {
        private Dictionary<string, Variable> Variables { get; set; }

        /// <summary>
        /// Initializes a new language engine
        /// </summary>
        public Engine()
        {
            Variables = new Dictionary<string, Variable>();
        }

        /// <summary>
        /// Declares a new variable, setting aside a place in memory for it.
        /// </summary>
        /// <param name="variable">The name of the variable to declare</param>
        /// <returns>true if the variable was successfully declared, false if the variable already existed or otherwise was not able to be declared</returns>
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

        /// <summary>
        /// Assigns the given value to the variable denoted by the given variable name.
        /// 
        /// If the variable is undeclared before a call to this method, this method declares the variable
        /// </summary>
        /// <param name="variable">The name of the variable to which to set the value</param>
        /// <param name="value">The value to assign to the variable</param>
        public void Assign(string variable, Variable value)
        {
            if (!Exists(variable))
            {
                Declare(variable);
            }
            Variables[variable] = value;
        }

        /// <summary>
        /// Returns the variable with the given name
        /// </summary>
        /// <param name="variable">the name of the variable to return</param>
        /// <returns>The value of the variable with the given name</returns>
        /// <exception cref="UndeclaredVariableException">the variable with the given name does not exist</exception>
        public Variable Get(string variable)
        {
            if (Exists(variable))
            {
                return Variables[variable];
            }
            throw new UndeclaredVariableException(variable);
        }

        /// <summary>
        /// Removes the variable with the given name from the available cache of variables.
        /// </summary>
        /// <param name="variable">the name of the variable to delete</param>
        /// <exception cref="UndeclaredVariableException">the variable with the given name does not exist</exception>
        public void Delete(string variable)
        {
            if (Exists(variable))
            {
                Variables.Remove(variable);
            }
            else
            {
                throw new UndeclaredVariableException(variable);
            }
        }

        /// <summary>
        /// Returns true if the variable with the given name has been declared, returns false otherwise
        /// </summary>
        /// <param name="variable">The name of the variable to check</param>
        /// <returns>true if the variable exists, false otherwise</returns>
        public bool Exists(string variable)
        {
            return Variables.Keys.Any(v => v == variable);
        }
    }
}
