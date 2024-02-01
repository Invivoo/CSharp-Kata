using FluentAssertions;
using Xunit;
public class ResultTests
{
    [Fact]
    public void PartitionToString_Should_ReturnExpected()
    {
        // Arrange
        var tested = CreatePartition(4, 3, 3);
        var expected = "[4, 3, 3]";

        // Act
        var actual = tested.ToString();

        // Assert
        actual.Should().Be(expected);
    }

    [Fact]
    public void PartitionProduct_Should_ReturnExpected()
    {
        // Arrange
        var tested = CreatePartition(4, 3, 3);
        var expected = 36;

        // Act
        var actual = tested.Product;

        // Assert
        actual.Should().Be(expected);
    }

    [Fact]
    public void ComputeProduct_Should_ReturnExpected()
    {
        // Arrange
        var tested = CreatePartition(4, 3, 3);
        var expected = 36;

        // Act
        var actual = tested.ComputeProduct();

        // Assert
        actual.Should().Be(expected);
    }

    [Fact]
    public void ResultToString_When_Single_Should_ReturnExpected()
    {
        // Arrange
        var partition = CreatePartition(4, 3, 3);
        var tested = new Result(36, partition);
        var expected = "([[4, 3, 3]], 36)";

        // Act
        var actual = tested.ToString();

        // Assert
        actual.Should().Be(expected);
    }

    [Fact]
    public void ResultToString_When_Multiple_Should_ReturnExpected()
    {
        // Arrange
        var partition1 = CreatePartition(4, 3, 3);
        var partition2 = CreatePartition(3, 3, 2, 2);
        var tested = new Result(36, partition1, partition2);
        var expected = "([[4, 3, 3], [3, 3, 2, 2]], 36)";

        // Act
        var actual = tested.ToString();

        // Assert
        actual.Should().Be(expected);
    }

    private Partition CreatePartition(params int[] values)
    {
        return new Partition(values.ToList());
    }
}