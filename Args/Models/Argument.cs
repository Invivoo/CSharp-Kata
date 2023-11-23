using System;
using System.Collections.Generic;
using System.Linq;

namespace Args.Models;

public record Param(bool ShouldLog, int Port, IEnumerable<int> MyNumerics, IEnumerable<string> MyStrings)
{
    public Param(bool shouldLog, int port) : this(shouldLog, port, Array.Empty<int>(), Array.Empty<string>()) { }
    public Param(bool shouldLog, int port, IEnumerable<int> MyNumerics) : this(shouldLog, port, MyNumerics, Array.Empty<string>()) { }
}

public abstract class Argument
{
    public string Key { get; init; }
    public abstract int AdditionalIncrement { get; }

    public Argument(string key)
    {
        Key = key;
    }

    public abstract Param Parse(string[] input, int position, Param parameter);
}

public class LogArgument : Argument
{
    public LogArgument() : base("-l") { }

    public override int AdditionalIncrement => 0;

    public override Param Parse(string[] input, int position, Param parameter) => parameter with { ShouldLog = true };
}

public class PortArgument : Argument
{
    public override int AdditionalIncrement => 1;

    public PortArgument() : base("-p") { }

    public override Param Parse(string[] input, int position, Param parameter) => parameter with { Port = ParsePort(input[position + 1]) };
    private static int ParsePort(string port)
    {
        if (int.TryParse(port, out var result))
        {
            if (result < 0 || result > 65535)
                throw new ArgumentException($"'{port}' is not in the expected range for a port number.");
            return result;
        }
        throw new ArgumentException($"'{port}' is not a valid value for a port number.");
    }
}

public class IntegerArrayArgument : Argument
{
    public IntegerArrayArgument() : base("-i") { }

    public override int AdditionalIncrement => 1;

    public override Param Parse(string[] input, int position, Param parameter) => parameter with {  MyNumerics = input[position+1].Split(',').Select(x => int.Parse(x))};
}

public class StringArrayArgument : Argument
{
    public StringArrayArgument() : base("-g") { }

    public override int AdditionalIncrement => 1;
    
    public override Param Parse(string[] input, int position, Param parameter) => parameter with { MyStrings = input[position + 1].Split(',') };
}

