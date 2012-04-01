using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FluentCEngine
{
    public class Variable
    {

        private string _data;

        public dynamic Data
        {
            get
            {
                decimal result;
                if (decimal.TryParse(_data, out result))
                {
                    return result;
                }
                return _data;
            }

            set
            {
                _data = value.ToString();
            }
        }

        public VarType Type { get; set; }

        public bool IsNumber { get { return Type == VarType.Number; } }
        public bool IsString { get { return Type == VarType.String; } }
        public bool IsCondition { get { return Type == VarType.Condition; } }

    }

    public enum VarType
    {
        Number, String, Condition
    }
}
