﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentCEngine.Interfaces;

namespace FluentCEngine.Constructs
{
    public class NativeVoidFunction : VoidFunction
    {
        private Action<object[]> Function { get; set; }

        public NativeVoidFunction(Action<object[]> function, params ParameterMetaData[] parameters)
        {
            Function = function;
            Parameters = parameters;
        }

        public void Run(params object[] parameters)
        {
            Function(parameters);
        }

        public IEnumerable<ParameterMetaData> Parameters { get; private set; }
    }
}
