using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Speech;
using System.Speech.Recognition;
using System.Speech.Synthesis;

namespace FluentC
{
    class Program
    {
        static FluentCParser parser = new FluentCParser() { SpeakOnTellMe = true };
        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                parser.RunFile(args[0]);
            }

            SpeechRecognizer recognizer = new SpeechRecognizer();

            var builder = new GrammarBuilder("Let ");
            builder.AppendDictation();
            builder.Append("exist");
            var declare = new Grammar(builder);
            declare.SpeechRecognized += recognizer_SpeechRecognized;
            recognizer.LoadGrammar(declare);

            builder = new GrammarBuilder("Let ");
            builder.AppendDictation();
            builder.Append(" be ");
            var choices = new Choices();
            for (int i = 0; i <= 1000; i++)
            {
                choices.Add(i.ToString());
            }
            builder.Append(choices);

            var defaultAssignment = new Grammar(builder) { Name = "Default Assignment" };
            defaultAssignment.SpeechRecognized += recognizer_SpeechRecognized;

            recognizer.LoadGrammar(defaultAssignment);

            var operationBuilder = new GrammarBuilder();

            var operationChoices = new Choices();
            operationChoices.Add(new GrammarBuilder(new SemanticResultValue(" plus ", " + ")));
            operationChoices.Add(new GrammarBuilder(new SemanticResultValue(" minus ", " - ")));
            operationChoices.Add(new GrammarBuilder(new SemanticResultValue(" times ", " * ")));
            operationChoices.Add(new GrammarBuilder(new SemanticResultValue(" divided by ", " / ")));

            operationBuilder.Append(operationChoices);

            operationBuilder.Append(choices);
            builder.Append(operationBuilder);

            var operation = new Grammar(builder);
            operation.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(operation_SpeechRecognized);
            recognizer.LoadGrammar(operation);

            builder = new GrammarBuilder("Tell me ");
            builder.AppendDictation();
            var tellme = new Grammar(builder);
            tellme.SpeechRecognized += recognizer_SpeechRecognized;
            recognizer.LoadGrammar(tellme);

            
            string entry = "";
            while (entry != "exit?")
            {
                Console.Write(">");
                entry = Console.ReadLine();
                Run(entry);
            }
            recognizer.Dispose();
        }

        static void operation_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            var semantics = e.Result.Semantics;
            var spacemaker = (e.Result.Semantics.Value.ToString() == " / ") ? 4 : 3;
            var text = e.Result.Words[0].Text.Substring(0, 1) + e.Result.Words[0].Text.Substring(1) + " ";
            for (int i = 1; i < e.Result.Words.Count - spacemaker; i++)
            {
                text += e.Result.Words[i].Text + " ";
            }
            text += e.Result.Words[e.Result.Words.Count-spacemaker].Text;
            text += semantics.Value;
            text += e.Result.Words[e.Result.Words.Count - 1].Text;
            text += ".";
            Console.WriteLine(text);
            Run(text);
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
