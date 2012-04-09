using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentCEngine;
using System.IO;

namespace FluentC
{
    /// <summary>
    /// Contains methods with which to run scripts written in accordance with the FluentC specification
    /// </summary>
    public class FluentCParser
    {
        private Engine Engine { get; set; }

        /// <summary>
        /// Creates a new FluentCParser running on a fresh instance of Engine
        /// </summary>
        public FluentCParser() : this(new Engine()) { }

        /// <summary>
        /// Creates a new FluentCParser that runs on the given Engine
        /// </summary>
        /// <param name="engine">The engine on which to run</param>
        public FluentCParser(Engine engine)
        {
            Engine = engine;
        }

        /// <summary>
        /// Runs the script contained within the given string
        /// </summary>
        /// <param name="script">the script to run</param>
        public void Run(string script)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Runs the script contained within the file denoted by the given filename
        /// </summary>
        /// <param name="filename">the name of the file to run as a script</param>
        public void RunFile(string filename)
        {
            Run(File.ReadAllText(filename));
        }
    }
}
