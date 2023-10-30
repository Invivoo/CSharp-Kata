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
        Assert.True(actual.ShouldLog);
    }

    [Fact]
    public void InterpretParamsAsFalse()
    {
        var parser = new CmdLineParser();
        var actual = parser.Parse(new string[] { "toto" });
        Assert.False(actual.ShouldLog);
    }

    [Fact]
    public void InterpretAPort()
    {
        var parser = new CmdLineParser();
        var actual = parser.Parse(new string[] { "-p", "8080" });
        Assert.Equal(8080, actual.Port);
    }

    [Fact]
    public void InterpretAnIntergerList()
    {
        var parser = new CmdLineParser();
        var actual = parser.Parse(new string[] { "-d", "1,2,-3,6" });
        var expected = new []{1,2,-3,6};
        Assert.Equal(expected, actual.MyNumerics);
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

    private static IEnumerable<object[]> MultipleArgsParameters()
    {
        yield return new object[] { new string[] { "-p", "8080", "-l" }, new Param(true, 8080) };
        yield return new object[] { new string[] { "-l", "-p", "8080" }, new Param(true, 8080) };
        yield return new object[] { new string[] { "-l", "-p", "8080", "-d", "1,2,3" }, new Param(true, 8080, new int[]{1, 2, 3}) };
    }
}

public record Param(bool ShouldLog, int Port, IEnumerable<int> MyNumerics)
{
    public Param(bool shouldLog, int port) : this(shouldLog, port, Array.Empty<int>()) { }
}

internal class CmdLineParser
{
    /*Todo:
     * Rajouter -g + support de string
     * Ajouter Obligatoire et facultatif
     * ...
     * Gestion d'erreur
     * Ability to parse different type of input
     *  => Ajouter la possibité de configurer le parseur
     */
    private const int DefaultParam = 8080;

    internal Param Parse(params string[] args)
    {
        var record = new Param(false, DefaultParam);
        for (var index = 0; index < args.Length; index++)
        {
            if (args[index] == "-l")
                record = record with { ShouldLog = true };
            if (args[index] == "-p")
                record = record with { Port = ParsePort(args[++index]) };
            if (args[index] == "-d")
                record = record with { MyNumerics = ParseNumerics(args[++index]).ToArray()};
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
}
