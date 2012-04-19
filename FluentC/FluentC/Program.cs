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
            FluentCParser parser = new FluentCParser();
            string entry = "";
            while (entry != "exit?")
            {
                Console.Write(">");
                entry = Console.ReadLine();
                parser.Run(entry);

            }
        }
    }
}
