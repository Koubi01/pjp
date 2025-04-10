using System;
using System.Globalization;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using pjpproject;

namespace MyNamespace
{
    class Program
    {
        static void Main(string[] args)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            var fileName = "D:\\Progro\\pjpproject\\input.txt";
            Console.WriteLine("Parsing: " + fileName);
            var inputFile = new StreamReader(fileName);
            AntlrInputStream input = new AntlrInputStream(inputFile);
            PLCLexer lexer = new PLCLexer(input);
            CommonTokenStream tokens = new CommonTokenStream(lexer);
            PLCParser parser = new PLCParser(tokens);

            var errorListener = new VerboseListener();
            parser.RemoveErrorListeners();  // Smazání defaultních chybových posluchačů
            parser.AddErrorListener(errorListener);  // Přidání vlastního listeneru

            IParseTree tree = parser.program();
            if (parser.NumberOfSyntaxErrors == 0)
            {
                Console.WriteLine("Bez erroru");
                Console.WriteLine(tree.ToStringTree(parser));
                if (Errors.NumberOfErrors != 0)
                {
                    Errors.PrintAndClearErrors();
                }
                Console.WriteLine("Bez erroru");
            }
            
        }
    }
}
