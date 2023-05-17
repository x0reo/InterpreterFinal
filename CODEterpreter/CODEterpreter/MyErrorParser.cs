using Antlr4.Runtime;

namespace CODEterpreter.Exceptions;

public class MyErrorParser : BaseErrorListener
{
    public override void SyntaxError(IRecognizer recognizer, IToken offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e)
    {
        // Stop the parser
        base.SyntaxError(recognizer, offendingSymbol, line, charPositionInLine, msg, e);

        // Display the error message
        Console.Error.WriteLine($"Parser error: {msg} at line {line}, position {charPositionInLine}");

        // Exit the application
        Environment.Exit(1);
    }
}