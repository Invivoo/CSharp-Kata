using Xunit;

namespace Args;

public class A
{
    public int Toto { get; set; }
}
public record B(int Toto);
public static class Test
{
    public static bool CompareA()
    {
        var first = new A { Toto = 1 };
        var second = new A { Toto = 1 };

        return first == second;
    }
    public static bool CompareB()
    {
        var first = new B(Toto: 1 );
        var second = new B (Toto: 1);

        return first == second;
    }
}

public class CmdLineParserShould
{
    [Fact]
    public void InterpretParamsAsTrue()
    {
        var  parser = new CmdLineParser();
        var actual = parser.Parse(new string[] { "-l"});
        Assert.Equal(true, actual.ShouldLog);
    }

    [Fact]
    public void InterpretParamsAsFalse()
    {
        var parser = new CmdLineParser();
        var actual = parser.Parse(new string[] { "toto" });
        Assert.Equal(false, actual.ShouldLog);
    }

    [Fact]
    public void InterpretAPort()
    {
        var parser = new CmdLineParser();
        var actual = parser.Parse(new string[] { "-p", "8080" });
        Assert.Equal(8080, actual.Port);
    }

    [Fact]
    public void InterpretMultipleArgs()
    {
        var parser = new CmdLineParser();
        var actual = parser.Parse(new string[] { "-l", "-p", "8080" });
        Assert.Equal(new Param(true, 8080), actual);
    }
}

public record Param(bool ShouldLog, int Port);

internal class CmdLineParser
{
    public CmdLineParser()
    {
    }
    
    internal Param Parse(string[] vs)
    {
        var record = new Param(false, 0);
        for(var index = 0; index < vs.Length;index++)
        {
            if (vs[index] == "-l")
                record = record with { ShouldLog = true };
            if (vs[index] == "-p")
                record = record with { Port = int.Parse(vs[++index])};
        }
        return record;
    }
}
