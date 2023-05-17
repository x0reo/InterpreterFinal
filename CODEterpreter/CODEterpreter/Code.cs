using CODEterpreter.InterpreterGrammar;

namespace CODEterpreter;

public class Code : CodeBaseVisitor<object?>
{
    private readonly HelperFunctions _hf = new HelperFunctions();
    private Dictionary<string, object?> ColonCall { get; } = new();
    private Dictionary<string, object?> CharacterIdentifier { get; } = new();
    private Dictionary<string, object?> IntegerIdentifier { get; } = new();
    private Dictionary<string, object?> FloatIdentifier { get; } = new();
    private Dictionary<string, object?> BooleanIdentifier { get; } = new();

    public Code()
    {
        ColonCall["DISPLAY"] = new Func<object?[], object?>(Display);
        ColonCall["SCAN"] = new Func<object?[], object?>(Scan);
    }

    private object? Scan(object?[] arrItems)
    {
        Console.Write("\n");
        Console.Write("SCAN: ");
        var userInput = Console.ReadLine() ?? throw new InvalidOperationException();
        var userInputIdentifiers = userInput.Split(',');
        int varcnt = 0;
        if (userInputIdentifiers.Length != arrItems.Length)
        {
            Console.WriteLine("SCAN length format is incorrect");
            Environment.Exit(1);
        }
        foreach (var arrItem in arrItems)
        {
            if (CharacterIdentifier.ContainsKey(arrItem!.ToString()!))
            {
                var convInput = Convert.ToChar(userInputIdentifiers[varcnt]);
                CharacterIdentifier[arrItem.ToString()!] = convInput;
            }else if (IntegerIdentifier.ContainsKey(arrItem!.ToString()!))
            {
                var convInput = Convert.ToInt32(userInputIdentifiers[varcnt]);
                IntegerIdentifier[arrItem.ToString()!] = convInput;
            }else if (FloatIdentifier.ContainsKey(arrItem!.ToString()!))
            {
                var convInput = float.Parse(userInputIdentifiers[varcnt]);
                FloatIdentifier[arrItem.ToString()!] = convInput;
            }else if (BooleanIdentifier.ContainsKey(arrItem!.ToString()!))
            {
                var convInput = userInputIdentifiers[varcnt];
                
                if(convInput is "TRUE" or "FALSE")
                    BooleanIdentifier[arrItem.ToString()!] = convInput;
                else
                {
                    Console.WriteLine("Cannot recognize \"TRUE\" or \"FALSE\" value");
                    Environment.Exit(1);
                }
            }
            else
                throw new Exception("Identifier is not declared in the system");

            varcnt++;
        }  
        return null;
    }

    private object? Display(object?[] arrItems)
    {
        foreach (var arrItem in arrItems)
        {
            if(arrItem is null)
                throw new Exception("Error no value initialized");
            Console.Write(arrItem);
        }
        return null;
    }

 //done end
    public override object? VisitColonFunc(CodeParser.ColonFuncContext context)
    {
        if (context.COLONFUNCTION().ToString() == "DISPLAY")
        {
            var cfname = context.COLONFUNCTION().GetText();
            var express = context.expression().Select(Visit).ToArray();
            if(express.Length == 0)
                throw new Exception("No value/s received");
            if (ColonCall[cfname] is not Func<object?[], object?> func)
                throw new Exception($"{cfname}: is not a valid function");
            return func(express);
        }
        else
        {
            var cfname = context.COLONFUNCTION().GetText();
            var express = context.expression().Select(Visit).ToArray();
            
            if(express.Length == 0)
                throw new Exception($"No value/s received on scan");
            if (ColonCall[cfname] is not Func<object?[], object?> func)
                throw new Exception($"{cfname}: is not a valid function");
            return func(express);
        }
    }
    //done
    public override object? VisitDeclaration(CodeParser.DeclarationContext context)
    {
        var declarations =context.declarations().GetText();
        string[] identifiers = declarations.Split(',');
        
        var identLength = identifiers.Length;
        var dataType = context.DATATYPE().GetText();
        
        for (int i = 0; i < identLength; i++)
        {
            var current = identifiers[i];
            //example: x=0
            if (current.Contains('='))
            {
                //Get rid of the =
                var arrCurr = current.Split('=');
                var currIdentifierName = arrCurr[0];
                var currIdentifierValue = arrCurr[1];
                if (dataType == "CHAR")
                {
                    if (currIdentifierValue.Length != 3)
                    {
                        Console.WriteLine($"Not a valid char value {currIdentifierValue}");
                        Environment.Exit(1);
                    }     
                    if (currIdentifierValue[0] == '"')
                    {
                        Console.WriteLine($"Not a valid char value received a STRING {currIdentifierValue}");
                        Environment.Exit(1);
                    }

                    InitializeMain(dataType, currIdentifierName, currIdentifierValue[1..^1]);
                        
                }else if(dataType == "INT")
                {
                    var currInt = int.TryParse(currIdentifierValue, out var intResult);
                    if(currInt)
                        InitializeMain(dataType, currIdentifierName, intResult);
                    else
                    {
                        Console.WriteLine($"Not a valid integer value {currIdentifierValue}");
                        Environment.Exit(1);    
                    }
                        
                }else if(dataType == "FLOAT")
                {
                    var currFloat = float.TryParse(currIdentifierValue, out var floatResult);
                    if (currFloat)
                        InitializeMain(dataType, currIdentifierName, floatResult);
                    else
                    {
                        Console.WriteLine($"Not a valid floating value {currIdentifierValue}");
                        Environment.Exit(1);
                    }
                }else if (dataType == "BOOL")
                {
                    if (currIdentifierValue == "\"TRUE\"" || currIdentifierValue == "\"FALSE\"")
                        InitializeMain(dataType, currIdentifierName, currIdentifierValue[1..^1]);
                    else
                    {
                        Console.WriteLine($"Not a valid boolean value {currIdentifierValue}");
                        Environment.Exit(1);
                    }
                }
                else
                {
                    Console.WriteLine($"Error data type is not in the system {dataType}");
                    Environment.Exit(1);
                }
            }
            else
            {
                if (CharacterIdentifier.ContainsKey(current) || IntegerIdentifier.ContainsKey(current) ||
                    FloatIdentifier.ContainsKey(current) || BooleanIdentifier.ContainsKey(current))
                {
                    Console.WriteLine($"Variable already declared : {current}");
                    Environment.Exit(1);
                }
                InitializeMain(dataType, current, null);
            }
        }
        return null;
    }
    public override object? VisitIdentifierExpression(CodeParser.IdentifierExpressionContext context)
    {
        var identifier = context.IDENTIFIER().GetText();
        if (context.parent.GetChild(0).ToString() == "SCAN")
        {
            return identifier;
        }
        //check main
        if (CharacterIdentifier.TryGetValue(identifier, out var visitIdentifierExpression2))
        {
            return visitIdentifierExpression2;
        }
        if (IntegerIdentifier.TryGetValue(identifier, out var expression3))
        {
            return expression3;
        }
        if (FloatIdentifier.TryGetValue(identifier, out var identifierExpression3))
        {
            return identifierExpression3;
        }
        if (BooleanIdentifier.TryGetValue(identifier, out var visitIdentifierExpression3))
        {
            return visitIdentifierExpression3;
        }
        Console.WriteLine($"Variable {identifier} is not defined");
        Environment.Exit(1);
        return null;
    }
    //done
    private void InitializeMain(string dataType, string identifierName, object? identifierValue)
    {
        switch (dataType)
        {
            case "CHAR":
                CharacterIdentifier[identifierName] = identifierValue;
                break;
            case "INT":
                IntegerIdentifier[identifierName] = identifierValue;
                break;
            case "FLOAT":
                FloatIdentifier[identifierName] = identifierValue;
                break;
            case "BOOL":
                BooleanIdentifier[identifierName] = identifierValue;
                break;
        }
    }
    //done
    public override object? VisitAssignment(CodeParser.AssignmentContext context)
    {
        //x = -x
        var varUnarySign = context.GetChild(2).GetText();
        var identifierName = context.assignments().GetText();
        var express = Visit(context.expression());
        var identifierSplit = identifierName!.Split('=');
        foreach (var identItem in identifierSplit)
        {
            if (CharacterIdentifier.ContainsKey(identItem))
            {
                //if (value is string | value is char)
                if (express is string | express is char)
                {
                    if (varUnarySign == "-" | varUnarySign == "+" || express!.ToString()!.Length > 1)
                    {
                        Console.WriteLine($"Expected a char value: {varUnarySign}");
                        Environment.Exit(1);
                    }
                    CharacterIdentifier[identItem] = express;
                }
                else
                {
                    Console.WriteLine($"Error assignment for identifier {identifierName}: expected to be CHAR");
                    Environment.Exit(1);
                }
            }
            else if (IntegerIdentifier.ContainsKey(identItem))
            {
                if (express is int i)
                {
                    if (varUnarySign == "-")
                    { 
                        IntegerIdentifier[identItem] = -1 * i;
                    }else
                        IntegerIdentifier[identItem] = i;
                }
                else
                {
                    Console.WriteLine($"Error assignment for identifier {identifierName} : expected to be INT");
                    Environment.Exit(1);
                }
            }
            else if (FloatIdentifier.ContainsKey(identItem))
            {
                if (express is float f)
                {
                    if (varUnarySign == "-")
                        FloatIdentifier[identItem] = -1.0 * f;
                    else
                        FloatIdentifier[identItem] = f;
                }
                else
                {
                    Console.WriteLine($"Error assignment for identifier {identifierName}: expected to be FLOAT");
                    Environment.Exit(1);
                }
            }
            else if (BooleanIdentifier.ContainsKey(identItem))
            {
                if (express is "TRUE" or "FALSE")
                {
                    if (varUnarySign == "=" | varUnarySign == "+")
                    {
                        Console.WriteLine($"Error unary sign in a bool");
                        Environment.Exit(1);
                    }
                    BooleanIdentifier[identItem] = express;
                }
                else
                {
                    Console.WriteLine($"Error assignment for identifier {identifierName}: expected to be BOOL");
                    Environment.Exit(1);
                }
            }
            else
            {
                Console.WriteLine($"Error identifier {identItem} is not recognize.");
                Environment.Exit(1);
            }
        }
        return null; 
    }
    //done
    public override object? VisitConstant(CodeParser.ConstantContext context)
    {
        if (context.INTEGERLITERAL() != null)
            return int.Parse(context.parent.GetText());
        if (context.FLOATLITERAL() != null)
            return float.Parse(context.parent.GetText());
        if (context.CHARLITERAL() is {} c)
            return c.GetText()[1..^1];
        if (context.BOOLEANLITERAL() != null)
            return context.BOOLEANLITERAL().GetText() == "TRUE";
        if (context.STRINGLITERAL() is { } s)
            return s.GetText()[1..^1];
        
        return null;
    }
    //done
    public override object? VisitNewLineExpression(CodeParser.NewLineExpressionContext context)
    {
        return context.DOLLARSIGNCARRIAGE() != null ? "\n" : null;
    }
    //done
    public override object? VisitConcatenateExpression(CodeParser.ConcatenateExpressionContext context)
    {
        return Visit(context.expression(0))!.ToString() + Visit(context.expression(1))!.ToString();
    }
    //done
    private static object? VisitErrorCaller(string message)
    {
        Console.WriteLine(message);
        Environment.Exit(1);
        return null;
    }
    public override object? VisitBinaryExpression(CodeParser.BinaryExpressionContext context)
    {
        var leftExpression = Visit(context.expression(0));
        var rightExpression = Visit(context.expression(1));
        var operation = context.binaryOperation().GetText();
        return operation switch
        {
            "+" => _hf.Addition(leftExpression, rightExpression),
            "-" => _hf.Subtraction(leftExpression, rightExpression),
            "*" => _hf.Multiplication(leftExpression, rightExpression),
            "/" => _hf.Division(leftExpression, rightExpression),
            "%" => _hf.Modulo(leftExpression, rightExpression),
            _ => VisitErrorCaller("Operation is not valid")
        };
    }
    //done
    public override object? VisitBooleanExpression(CodeParser.BooleanExpressionContext context)
    {
        var leftExpression = Visit(context.expression(0));
        var rightExpression = Visit(context.expression(1));
        var operation = context.booleanOperation().GetText();

        return operation switch
        {
            "<" => _hf.LessThan(leftExpression, rightExpression),
            ">" => _hf.GreaterThan(leftExpression, rightExpression),
            "<=" => _hf.LessEqual(leftExpression, rightExpression),
            ">=" => _hf.GreatEqual(leftExpression, rightExpression),
            "==" => _hf.EqualEqual(leftExpression, rightExpression),
            "<>" => _hf.NotEqual(leftExpression, rightExpression),
            _ => VisitErrorCaller("Operation is not valid")//throw new Exception("Operation is not valid")
        };
    }
    
    public override object? VisitParenthesizedExpression(CodeParser.ParenthesizedExpressionContext context)
    {
        return Visit(context.expression());
    }

    public override object? VisitLogicalExpression(CodeParser.LogicalExpressionContext context)
    {
        var leftExpression = Visit(context.expression(0));
        var rightExpression = Visit(context.expression(1));
        var operation = context.logicalOperation().GetText();
        return operation switch
        {
            "AND" => _hf.AndLogic(leftExpression, rightExpression),
            "OR" => _hf.OrLogic(leftExpression, rightExpression),
            _ => VisitErrorCaller("Operation is not valid")
        };
    }

    public override object? VisitNotBoolExpression(CodeParser.NotBoolExpressionContext context)
    {
        var val = Visit(context.expression());
        return val!.ToString() == "TRUE" ? "FALSE" : "TRUE";
    }
    
    public override object? VisitIfBlock(CodeParser.IfBlockContext context)
    {
        var express = Visit(context.expression());
        var xw = context.GetText();
        switch (express!.ToString())
        {
            case "TRUE":
                Visit(context.block());
                break;
            case "FALSE":
            {
                if (context.elseIfBlock() != null)
                {
                    Visit(context.elseIfBlock());
                }
                break;
            }
            default:
                Console.WriteLine($"Expecting a boolean expression {express}");
                Environment.Exit(1);
                break;
        }
        return null;
    }

    public override object? VisitBlock(CodeParser.BlockContext context)
    {
        return Visit(context.blockLine());
    }

    public override object? VisitElseIfBlock(CodeParser.ElseIfBlockContext context)
    {
        if (context.block() != null)
            return Visit(context.block());
        return context.ifBlock() != null ? Visit(context.ifBlock()) : null;
    }

    public override object? VisitWhileBlock(CodeParser.WhileBlockContext context)
    {
        
        Func<object?, bool> condition = ProcessWhile;
        while (condition(Visit(context.expression())))
        {
            Visit(context.inWhileBlock());
        }
        return null;
    }

    private bool ProcessWhile(object? value)
    {
        switch (value!.ToString())
        {
            case "TRUE":
                return true;
            case "FALSE":
                return false;
            default:
                Console.WriteLine("Value is not a boolean");
                Environment.Exit(1);
                break;
        }

        return false;
    }
    public override object? VisitInWhileBlock(CodeParser.InWhileBlockContext context)
    {
        return Visit(context.lines());
    }
}