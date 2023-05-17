namespace CODEterpreter;

public class HelperFunctions
{
    public HelperFunctions(){}

    public object? OrLogic(object? leftIdentifierValue, object? rightIdentifierValue)
    {
        if (leftIdentifierValue!.ToString()!.ToUpper() == "FALSE" && rightIdentifierValue!.ToString()!.ToUpper() == "FALSE")
            return "FALSE";
        return "TRUE";
    }

    public object? AndLogic(object? leftIdentifierValue, object? rightIdentifierValue)
    {
        if (leftIdentifierValue!.ToString()!.ToUpper() == "TRUE" && rightIdentifierValue!.ToString()!.ToUpper() == "FALSE"
            || leftIdentifierValue!.ToString()!.ToUpper() == "FALSE" && rightIdentifierValue!.ToString()!.ToUpper() == "TRUE"
            || leftIdentifierValue!.ToString()!.ToUpper() == "FALSE" && rightIdentifierValue!.ToString()!.ToUpper() == "FALSE")
            return "FALSE";
        return "TRUE";
    }

    public object? Modulo(object? leftIdentifierValue, object? rightIdentifierValue)
    {
        if (leftIdentifierValue is int l && rightIdentifierValue is int r)
        {
            return l % r;
        }

        if (leftIdentifierValue is float lf && rightIdentifierValue is float rf)
        {
            return lf % rf;
        }
        throw new Exception($"Error doing operation for: {leftIdentifierValue} and {rightIdentifierValue}");
    }

    public object? Division(object? leftIdentifierValue, object? rightIdentifierValue)
    {
        
        object? retVal = null;
        try
        {
            if (leftIdentifierValue is int l && rightIdentifierValue is int r)
            {
                retVal = l / r;
            }
            if (leftIdentifierValue is float lf && rightIdentifierValue is float rf)
            {
                retVal = lf / rf;
            }
        }
        catch (ArithmeticException ex)
        {
            Console.WriteLine("Message: " + ex.Message);
            Environment.Exit(1);
        }

        return retVal;
    }

    public object? Multiplication(object? leftIdentifierValue, object? rightIdentifierValue)
    {
        if (leftIdentifierValue is int l && rightIdentifierValue is int r)
        {
            return l * r;
        }

        if (leftIdentifierValue is float lf && rightIdentifierValue is float rf)
        {
            return lf * rf;
        }
        throw new Exception($"Error doing operation for: {leftIdentifierValue} and {rightIdentifierValue}");
    }
    
    public object? Addition(object? leftIdentifierValue, object? rightIdentifierValue)
    {   
        if (leftIdentifierValue is int l && rightIdentifierValue is int r)
        {
            return l + r;
        }
        
        if (leftIdentifierValue is int litf && rightIdentifierValue is float rfti)
        {
            return litf + rfti;
        }
        
        if (leftIdentifierValue is float lfti && rightIdentifierValue is int ritf)
        {
            return lfti + ritf;
        }

        if (leftIdentifierValue is float lf && rightIdentifierValue is float rf)
        {
            return lf + rf;
        }

        throw new Exception($"Error doing operation for: {leftIdentifierValue} and {rightIdentifierValue}");
    }
    public object? Subtraction(object? leftIdentifierValue, object? rightIdentifierValue)
    {
        if (leftIdentifierValue is int l && rightIdentifierValue is int r)
        {
            return l - r;
        }

        if (leftIdentifierValue is float lf && rightIdentifierValue is float rf)
        {
            return lf - rf;
        }
        throw new Exception($"Error doing operation for: {leftIdentifierValue} and {rightIdentifierValue}");
    }
    
  public object? NotEqual(object? leftIdentifierValue, object? rightIdentifierValue)
    {
        if (leftIdentifierValue!.ToString()!.ToUpper().Equals(rightIdentifierValue!.ToString()!.ToUpper()))
            return "FALSE";
        return "TRUE";
    }

  public object? EqualEqual(object? leftIdentifierValue, object? rightIdentifierValue)
  {
      if (leftIdentifierValue!.ToString()!.ToUpper().Equals(rightIdentifierValue!.ToString()!.ToUpper()))
          return "TRUE";
      return "FALSE";
    }

  public object? GreatEqual(object? leftIdentifierValue, object? rightIdentifierValue)
    {
        if (leftIdentifierValue is int l && rightIdentifierValue is int r)
        {
            return l >= r ? "TRUE" : "FALSE";
        }

        if (leftIdentifierValue is float lf && rightIdentifierValue is float rf)
        {
            return lf >= rf ? "TRUE" : "FALSE";
        }
        throw new Exception($"Error doing operation for: {leftIdentifierValue} and {rightIdentifierValue}");
    }

  public object? LessEqual(object? leftIdentifierValue, object? rightIdentifierValue)
    {
        if (leftIdentifierValue is int l && rightIdentifierValue is int r)
        {
            return l <= r ? "TRUE" : "FALSE";
        }

        if (leftIdentifierValue is float lf && rightIdentifierValue is float rf)
        {
            return lf <= rf ? "TRUE" : "FALSE";
        }
        throw new Exception($"Error doing operation for: {leftIdentifierValue} and {rightIdentifierValue}");
    }

  public object? GreaterThan(object? leftIdentifierValue, object? rightIdentifierValue)
    {
        if (leftIdentifierValue is int l && rightIdentifierValue is int r)
        {
            return l > r ? "TRUE" : "FALSE";
        }

        if (leftIdentifierValue is float lf && rightIdentifierValue is float rf)
        {
            return lf > rf ? "TRUE" : "FALSE";
        }
        throw new Exception($"Error doing operation for: {leftIdentifierValue} and {rightIdentifierValue}");
    }

  public object? LessThan(object? leftIdentifierValue, object? rightIdentifierValue)
    {
        if (leftIdentifierValue is int l && rightIdentifierValue is int r)
        {
            return l < r ? "TRUE" : "FALSE";
        }

        if (leftIdentifierValue is float lf && rightIdentifierValue is float rf)
        {
            return lf < rf ? "TRUE" : "FALSE";
        }
        throw new Exception($"Error doing operation for: {leftIdentifierValue} and {rightIdentifierValue}");
    }
}