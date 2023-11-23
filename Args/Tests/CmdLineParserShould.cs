using Args.Models;
using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;

namespace Args.Logic;

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
    
    private static IEnumerable<object[]> MultipleArgsParameters()
    {
        yield return new object[] { new string[] { "-p", "8080", "-l" }, new Param(true, 8080) };
        yield return new object[] { new string[] { "-l", "-p", "8080" }, new Param(true, 8080) };
        yield return new object[] { new string[] { "-l", "-p", "8080", "-i", "1,2,3" }, new Param(true, 8080, new int[]{1, 2, 3}) };
    }
}