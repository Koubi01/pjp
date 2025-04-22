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
            var fileName = "C:\\Users\\Hupin\\OneDrive\\Plocha\\kubaSkola\\pjp\\input.txt";
            Console.WriteLine("Parsing: " + fileName);
            var inputFile = new StreamReader(fileName);
            AntlrInputStream input = new AntlrInputStream(inputFile);
            PLCLexer lexer = new PLCLexer(input);
            CommonTokenStream tokens = new CommonTokenStream(lexer);
            PLCParser parser = new PLCParser(tokens);

           // var errorListener = new VerboseListener();
            parser.RemoveErrorListeners();  
           // parser.AddErrorListener(errorListener); 

            var tree = parser.program();
           // var typeChecker = new TypeCheckerVisitor();
            //typeChecker.Visit(tree);


            if (Errors.NumberOfErrors > 0)
            {
                Console.WriteLine("Type checking failed:");
                Errors.PrintAndClearErrors();
                return;
            }

            Console.WriteLine("Type checking passed.");

            var codeGen = new CodeGenVisitor();
            codeGen.Visit(tree);
            File.WriteAllLines("output.txt", codeGen.GetInstructions());

            var inter = new Interpreter();
            inter.InterpretFile("output.txt");
            
            return;
                        
        }
    }
}
