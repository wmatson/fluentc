using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FluentC
{
    class Program
    {
        static void Main(string[] args)
        {
            new FluentCParser().Run("Let assign be 2. Let declare exist. Forget delete.");
            Console.ReadLine();
        }
    }
}
