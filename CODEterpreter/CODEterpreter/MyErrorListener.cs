using Antlr4.Runtime;

namespace CODEterpreter.Exceptions;

public class MyErrorListener : IAntlrErrorListener<int>//BaseErrorListener
{
    public void SyntaxError(IRecognizer recognizer, int offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e)
    {
        // Reset the input stream
        recognizer.InputStream.Seek(0);

        // Display the error message
        Console.Error.WriteLine($"Lexer error: {msg} at line {line}, position {charPositionInLine}");

        // Exit the application
        Environment.Exit(1);
    }
}