using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

public class Result
{
    public int MaxValue {get; set;}
    public IEnumerable<Partition> ListPartitions { get; }

    public Result(int maxValue, Partition firstPartition, params Partition[] otherPartitions) 
        : this(maxValue, new[] { firstPartition }.Union(otherPartitions))
    {
    }

    public Result(int maxValue, IEnumerable<Partition> partitions)
    {
        this.MaxValue = maxValue;
        this.ListPartitions = partitions.ToList();
    }

    public Result() : this(0, Enumerable.Empty<Partition>())
    {
    }

    public override string ToString()
    {
        return $"([{string.Join(", ", ListPartitions)}], {MaxValue})";
    }
}

public class Partition(IEnumerable<int> values)
{
    public IEnumerable<int> Values { get; } = values;

    public int Product => Values.Aggregate(1, (res, val) => res * val);

    public override string ToString() => $"[{string.Join(", ", Values)}]";
}