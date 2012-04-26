using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Speech;
using System.Speech.Recognition;

namespace FluentC
{
    class Program
    {
        static FluentCParser parser = new FluentCParser();
        static void Main(string[] args)
        {
            SpeechRecognizer recognizer = new SpeechRecognizer();
            recognizer.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(recognizer_SpeechRecognized);

            var builder = new GrammarBuilder("Let ");
            builder.AppendDictation();
            builder.Append("exist");
            recognizer.LoadGrammar(new Grammar(builder));

            builder = new GrammarBuilder("Let ");
            builder.AppendDictation();
            builder.Append(" be ");
            var choices = new Choices();
            for (int i = 0; i <= 1000; i++)
            {
                choices.Add(i.ToString());
            }
            builder.Append(choices);

            recognizer.LoadGrammar(new Grammar(builder));

            var operationBuilder = new GrammarBuilder();

            var operationChoices = new Choices();
            operationChoices.Add(new GrammarBuilder(new SemanticResultValue(" plus ", " + ")));
            operationChoices.Add(new GrammarBuilder(new SemanticResultValue(" minus ", " - ")));
            operationChoices.Add(new GrammarBuilder(new SemanticResultValue(" times ", " * ")));
            operationChoices.Add(new GrammarBuilder(new SemanticResultValue(" divided by ", " / ")));

            operationBuilder.Append(operationChoices);

            operationBuilder.Append(choices);
            builder.Append(operationBuilder);

            recognizer.LoadGrammar(new Grammar(builder));

            builder = new GrammarBuilder("Tell me ");
            builder.AppendDictation();
            recognizer.LoadGrammar(new Grammar(builder));

            
            string entry = "";
            while (entry != "exit?")
            {
                Console.Write(">");
                entry = Console.ReadLine();
                Run(entry);
            }
        }

        static void recognizer_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            var text = "";
            text +=  e.Result.Text + ".";
            Console.WriteLine(text);
            Run(text);
        }

        static void Run(string entry)
        {
            try
            {
                parser.Run(entry);
            }
            catch (Exception e)
            {
                Console.WriteLine("Your entry caused an error: {0}", e.Message);
            }
        }
    }
}
