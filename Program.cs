using System;
using System.Globalization;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;

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

            //parser.AddErrorListener(new VerboseListener());

            IParseTree tree = parser.program();
            Console.WriteLine(tree.ToStringTree(parser));
        }
    }
}
