using FluentAssertions;
using Xunit;

public class CreatePartitionTests
{
    [Fact]
    public void ShouldReturn()
    {
        var expected = new Partition[] 
        { 
            new (8), 
            new (7, 1), 
            new (6, 1, 1),
            new (5, 1, 1, 1),
            new (4, 1, 1, 1, 1),
            new (3, 1, 1, 1, 1, 1),
            new (2, 1, 1, 1, 1, 1, 1),
            new (1, 1, 1, 1, 1, 1, 1, 1),
        };

        var actual = PartionHelper.CreatePartitions(8).ToArray();

        foreach(var partition in expected)
        {
            actual.Should().ContainEquivalentOf(partition);
        }
    }

    //Todo: make the test pass
    [Fact]
    public void ShouldReturn2()
    {
        var expected = new Partition[]
        {
            new (2, 2, 2, 2),
            new (2, 2, 2, 1, 1),
            new (2, 2, 1, 1, 1, 1),
            new (2, 1, 1, 1, 1, 1, 1),
        };

        var actual = PartionHelper.CreatePartitions(8).ToArray();

        actual.Should().Contain(expected);
    }
}
