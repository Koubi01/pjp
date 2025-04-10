using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using System;
using System.Collections.Generic;
using System.Linq;


namespace pjpproject
{
    public class VerboseListener : BaseErrorListener
    {
        public override void SyntaxError(
            TextWriter output,
            IRecognizer recognizer,
            IToken offendingSymbol,
            int line,
            int charPositionInLine,
            string msg,
            RecognitionException e)
        {
            // Formátování chybové zprávy
            string fullMsg = $"line {line}:{charPositionInLine} at '{offendingSymbol.Text}': {msg}";

            // Volání metody pro záznam chyby
            Errors.ReportError(offendingSymbol, fullMsg);

            // Výstup chyby na chybovou konzoli
            output.WriteLine(fullMsg);  // Mělo by použít Console.Error místo output, pokud chceš psát přímo do konzole
        }
    }
}
