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

            string fullMsg = $"line {line}:{charPositionInLine} at '{offendingSymbol.Text}': {msg}";

            Errors.ReportError(offendingSymbol, fullMsg);

            output.WriteLine(fullMsg);
        }
    }
}
