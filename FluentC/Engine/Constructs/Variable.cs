using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FluentCEngine.Constructs
{
    /// <summary>
    /// Represents a variable of any type supported by Engine
    /// </summary>
    /// <seealso cref="Engine"/>
    public class Variable
    {

        private string _data;

        public dynamic Data
        {
            get
            {
                if (IsNumber)
                {
                    return decimal.Parse(_data);
                }
                return _data;
            }

            set
            {
                decimal result;
                if(!decimal.TryParse(value, out result)) {
                    Type = VarType.Number;
                }
               _data = value.ToString();                
            }
        }

        public VarType Type { get; set; }

        public bool IsNumber { get { return Type == VarType.Number; } }
        public bool IsString { get { return Type == VarType.String; } }
        public bool IsCondition { get { return Type == VarType.Condition; } }

    }

    /// <summary>
    /// Represents every type of variable supported by engine
    /// </summary>
    public enum VarType
    {
        Number, String, Condition
    }
}
