using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentCEngine.Exceptions;
using FluentCEngine.Constructs;
using FluentCEngine.Interfaces;

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
        private Dictionary<string, VoidFunction> VoidFunctions { get; set; }

        /// <summary>
        /// Initializes a new language engine
        /// </summary>
        public Engine()
        {
            Variables = new Dictionary<string, Variable>();
            VoidFunctions = new Dictionary<string, VoidFunction>();
        }

        #region Variable Management
        /// <summary>
        /// Declares a new variable, setting aside a place in memory for it.
        /// </summary>
        /// <param name="variable">The name of the variable to declare</param>
        /// <returns>true if the variable was successfully declared, false if the variable already existed or otherwise was not able to be declared</returns>
        public bool Declare(string variable)
        {
            try
            {
                Variables.Add(variable, new Variable());
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
        public void Assign(string variable, dynamic value)
        {
            if (!Exists(variable))
            {
                Declare(variable);
            }
            Variables[variable].Data = value;
        }

        /// <summary>
        /// Returns the variable with the given name
        /// </summary>
        /// <param name="variable">the name of the variable to return</param>
        /// <returns>The variable with the given name</returns>
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
        /// Returns the value of the variable with the given name
        /// </summary>
        /// <param name="variable">the name of the variable whose value to return</param>
        /// <returns>the value of the variable with the given name</returns>
        /// <exception cref="UndeclaredVariableException">the variable with the given name does not exist</exception>
        public dynamic GetValue(string variable)
        {
            return Get(variable).Data;
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
        #endregion

        #region VoidFunction Management

        /// <summary>
        /// Returns true if the function with the given name has been declared, returns false otherwise
        /// </summary>
        /// <param name="function">The name of the function to check</param>
        /// <returns>true if the function exists, false otherwise</returns>
        public bool VoidFunctionExists(string function)
        {
            return VoidFunctions.Keys.Any(v => v == function);
        }

        public void RunVoidFunction(string function, params object[] parameters)
        {
            VoidFunctions[function].Run(parameters);
        }

        /// <summary>
        /// Declares the given void function, assigning to it the Vvalue
        /// </summary>
        /// <param name="function">The name under which to declare the function</param>
        /// <param name="value">The value to assign to the function</param>
        public void DeclareVoidFunction(string function, VoidFunction value)
        {
            if (!VoidFunctionExists(function))
            {
                VoidFunctions[function] = value;
            }
            else
            {
                throw new DuplicateNameException(function);
            }
        }

        #endregion
    }
}
