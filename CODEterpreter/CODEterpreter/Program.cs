using Antlr4.Runtime;
using CODEterpreter;
using CODEterpreter.Exceptions;
using CODEterpreter.InterpreterGrammar;

var contents =
    File.ReadAllText(
        "C:\\Users\\a\\Desktop\\Final\\CODEterpreter\\CODEterpreter\\Test.ci");
var program = contents.Trim();
var inputStream = new AntlrInputStream(program);
var codeLexer = new CodeLexer(inputStream);
codeLexer.RemoveErrorListeners();
codeLexer.AddErrorListener(new MyErrorListener());
//codeLexer.AddErrorListener(new LexerException());
var commonTokenStream = new CommonTokenStream(codeLexer);
var codeParser = new CodeParser(commonTokenStream);
codeParser.RemoveErrorListeners();
codeParser.AddErrorListener(new MyErrorParser());
//codeParser.AddErrorListener(ParserException.Instance);
var startAst = codeParser.program();
var visitor = new Code();

visitor.Visit(startAst);