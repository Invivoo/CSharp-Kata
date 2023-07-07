using System;
using System.Collections.Generic;
using Xunit;

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

    [Theory]
    [MemberData(nameof(MultipleArgsParameters))]
    public void InterpretMultipleArgs(string[] args, Param expected)
    {
        var parser = new CmdLineParser();
        var actual = parser.Parse(args);
        Assert.Equal(expected, actual);
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
    }
}

public record Param(bool ShouldLog, int Port);

//Nom : obligatoire, Prenom : obligatoire
public record UserParam(string Nom, string Prenom);
internal class CmdLineParser
{
    /*Todo:
     * Rajouter -d
     * Rajouter -g + support de string
     * Ajouter Obligatoire et facultatif
     * ...
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
}
