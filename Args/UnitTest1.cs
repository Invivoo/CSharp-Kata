using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using FluentAssertions;
namespace Args;

public class CmdLineParserShould
{
    [Fact]
    public void InterpretParamsAsTrue()
    {
        var parser = new CmdLineParser();
        var actual = parser.Parse(new string[] { "-l" });
        actual.ShouldLog.Should().Be(true);
    }

    [Fact]
    public void InterpretParamsAsFalse()
    {
        var parser = new CmdLineParser();
        var actual = parser.Parse(new string[] { "toto" });
        actual.ShouldLog.Should().Be(false);
    }

    [Fact]
    public void InterpretAPort()
    {
        var parser = new CmdLineParser();
        var actual = parser.Parse(new string[] { "-p", "8080" });
        actual.Port.Should().Be(8080);
    }

    [Fact]
    public void InterpretAnIntergerList()
    {
        var parser = new CmdLineParser();
        var actual = parser.Parse(new string[] { "-i", "1,2,-3,6" });
        var expected = new []{1,2,-3,6};
        actual.MyNumerics.Should().BeEquivalentTo(expected);
    }
    
    [Fact]
    public void InterpretStringList()
    {
        var parser = new CmdLineParser();
        var actual = parser.Parse(new string[] { "-g", "this,is,a,list" });
        var expected = new[] { "this","is", "a", "list" };
        actual.MyStrings.Should().BeEquivalentTo(expected);
    }
    
    [Theory]
    [MemberData(nameof(MultipleArgsParameters))]
    public void InterpretMultipleArgs(string[] args, Param expected)
    {
        var parser = new CmdLineParser();
        var actual = parser.Parse(args);
        actual.Should().BeEquivalentTo(expected);
    }

    [Theory]
    [InlineData("toto", "'toto' is not a valid value for a port number.")]
    [InlineData("-1", "'-1' is not in the expected range for a port number.")]
    [InlineData("65536", "'65536' is not in the expected range for a port number.")]
    public void WrongPortInputShouldThrow(string portInput, string expectedMessage)
    {
        var parser = new CmdLineParser();
        var exception = Assert.Throws<ArgumentException>(() => parser.Parse("-p", portInput));
        Assert.Equal(expectedMessage, exception.Message);
    }

    [Fact]
    public void InterpretDefault()
    {
        var parser = new CmdLineParser();
        var actual = parser.Parse();
        Assert.Equal(new Param(false, 8080), actual);
    }
// // 
    private static IEnumerable<object[]> MultipleArgsParameters()
    {
        yield return new object[] { new string[] { "-p", "8080", "-l" }, new Param(true, 8080) };
        yield return new object[] { new string[] { "-l", "-p", "8080" }, new Param(true, 8080) };
        yield return new object[] { new string[] { "-l", "-p", "8080", "-i", "1,2,3" }, new Param(true, 8080, new int[]{1, 2, 3}) };
    }
}

public record Param(bool ShouldLog, int Port, IEnumerable<int> MyNumerics, IEnumerable<string> MyStrings)
{
    public Param(bool shouldLog, int port) : this(shouldLog, port, Array.Empty<int>(), Array.Empty<string>()) { }
    public Param(bool shouldLog, int port, IEnumerable<int> MyNumerics) : this(shouldLog, port, MyNumerics, Array.Empty<string>()) { }
}

//public record Argument(string Key, Func<string[], int, Param, Param> ParameterBuilder, int Increment = 1);

public class CChild
{
    public int Age { get; set; }
}


public abstract class Argument
{
    public string Key { get; init; }
    public abstract int Increment { get; }

    public Argument(string key)
    {
        Key = key;
    }

    public abstract Param Parse(string[] input, int position, Param parameter);
}
public class LogArgument : Argument
{
    public LogArgument() : base("-l") { }

    public override int Increment => 1;

    public override Param Parse(string[] input, int position, Param parameter) => parameter with { ShouldLog = true };
}

public class PortArgument : Argument
{
    public override int Increment => 2;

    public PortArgument() : base("-l") { }

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


internal class CmdLineParser
{
    /*Todo:
     * Refacto => remove the switch
     * Ajouter Obligatoire et facultatif
     * ...
     * Gestion d'erreur
     * Ability to parse different type of input
     *  => Ajouter la possibité de configurer le parseur
     */
    private const int DefaultParam = 8080;

    //private Dictionary<string, Argument> Parsers = new()
    //{
    //    { "-l", new ("-l", (input, position, parameter) => parameter with { ShouldLog = true }) },
    //    { "-p", new ("-p", (input, position, parameter) => parameter with { Port = ParsePort(input[position + 1]) }, 2) },
    //    { "-i", new ("-i", (input, position, parameter) => parameter with { MyNumerics = ParseNumerics(input[position+ 1]) },2) },
    //    { "-g", new ("-g", (input, position, parameter) => parameter with { MyStrings = ParseStrings(input[position+ 1]) },2) }
    //};

    //private Dictionary<string, Func<string, Param, Param>> keyValuePairs = new Dictionary<string, Func<string, Param, Param>>()
    //{
    //    { "-l", (input, parameter) => parameter with { ShouldLog = true } },
    //    { "-p", (input, record) => record with { Port = ParsePort(args[++index]) } },
    //    { "-i", (input, record) => record with { MyNumerics = ParseNumerics(args[++index])},
    //    { "-g", (input, record) => record with { MyStrings = ParseStrings(args[++index]) }
    //};

    //private static readonly Argument[] SupportedArguments = new
    //{

    //};

    internal Param Parse(params string[] args)
    {
        var record = new Param(false, DefaultParam);
        for (var index = 0; index < args.Length; index++)
        {
            switch(args[index])
            {
                case "-l":
                    record = record with { ShouldLog = true };
                    break;
                case "-p":
                    record = record with { Port = ParsePort(args[++index]) };
                    break;
                case "-i":
                    record = record with { MyNumerics = ParseNumerics(args[++index])};
                    break;
                case "-g":
                    record = record with { MyStrings = ParseStrings(args[++index]) };
                    break;
                default:
                    continue;
                    //throw new ArgumentException($"Unrecognize argument: {args[index]}");
            }
        }
        return record;
    }

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

    private static IEnumerable<int> ParseNumerics(string numerics) => numerics.Split(',').Select(x => int.Parse(x));
    private static IEnumerable<string> ParseStrings(string strings) => strings.Split(',');
}
