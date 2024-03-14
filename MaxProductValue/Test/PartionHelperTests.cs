using Xunit;
using FluentAssertions;

public class PartionHelperTests
{
    private static IEnumerable<Partition> GetExpectedPartitions()
    {
        yield return new Partition(3, 3, 2);
    }

    [Fact]
    public void GetHighestProductShouldResturnExpected()
    {
        var parameter = BuildPartition();
        var actual = PartionHelper.GetHighestProduct(parameter);
        var expectedPartition = GetExpectedPartitions();
        actual.Should().BeEquivalentTo(expectedPartition);
    }
    [Fact]
    public void GetPartitionsShouldReturnExpected()
    {
        var actual = PartionHelper.CreatePartitions(8);
        var expectedPartitions = BuildPartition();
        actual.Should().BeEquivalentTo(expectedPartitions);
    }

    private static IEnumerable<Partition> BuildPartition()
    {
        yield return new Partition(8);
        yield return new Partition(7, 1);
        yield return new Partition(6, 2);
        yield return new Partition(6, 1, 1);
        yield return new Partition(5, 3);
        yield return new Partition(5, 2, 1);
        yield return new Partition(5, 1, 1, 1);
        yield return new Partition(4, 4);
        yield return new Partition(4, 3, 1);
        yield return new Partition(4, 2, 2);
        yield return new Partition(4, 2, 1, 1);
        yield return new Partition(4, 1, 1, 1, 1);
        yield return new Partition(3, 3, 1, 1);
        yield return new Partition(3, 3, 2);
        yield return new Partition(3, 2, 2, 1);
        yield return new Partition(3, 2, 1, 1, 1);
        yield return new Partition(3, 1, 1, 1, 1, 1);
        yield return new Partition(2, 2, 2, 2);
        yield return new Partition(2, 2, 2, 1, 1);
        yield return new Partition(2, 2, 1, 1, 1, 1);
        yield return new Partition(2, 1, 1, 1, 1, 1, 1);
        yield return new Partition(1, 1, 1, 1, 1, 1, 1, 1);
    }
}