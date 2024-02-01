using System;
using System.Collections.Generic;
using System.Linq;

namespace Args.Models;

public record Param(bool ShouldLog, int Port, IEnumerable<int> MyNumerics, IEnumerable<string> MyStrings, string? Me)
{
    public Param(bool shouldLog, int port, string me = "Tibo") : this(shouldLog, port, Array.Empty<int>(), Array.Empty<string>(), me) { }
    public Param(bool shouldLog, int port, IEnumerable<int> MyNumerics, string me = "Tibo") : this(shouldLog, port, MyNumerics, Array.Empty<string>(), me) { }
}

public abstract class Argument
{
    public string Key { get; init; }
    public abstract int AdditionalIncrement { get; }
    public bool Required { get; init; }

    public Argument(string key, bool required = false)
    {
        Key = key;
        Required = required;
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

public class WhoAmI : Argument
{
    public WhoAmI() : base("-me", true) { }
    public override int AdditionalIncrement => 1;
    public override Param Parse(string[] input, int position, Param parameter) => parameter with { Me = ParsePerson(input[position + 1]) };

    private static string ParsePerson(string person)
    {
        if (person.StartsWith("-l"))
            throw new Exception();
        if(person.Length < 2)
            throw new Exception();

        return person;
    }
}

public class MissingArgumentException : Exception
{
    public MissingArgumentException(string propertyName) : base()
    {
        PropertyName = propertyName;
    }

    public string PropertyName { get; }
}