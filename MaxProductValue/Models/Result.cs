using System.Collections.ObjectModel;
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

//public class PartitionBck(IEnumerable<int> values)
//{
//    public Partition(params int[] input) : this(input as IEnumerable<int>) 
//    { 
//    }
//    public Partition(int first, IEnumerable<int> middle, int last) : this(new[] { first }.Concat(middle).Concat(new[] {last}) as IEnumerable<int>) { }
//    public IEnumerable<int> Values { get; } = values;

//    public int Product => Values.Aggregate(1, (res, val) => res * val);

//    public override string ToString() => $"[{string.Join(", ", Values)}]";
//}

public class Partition(IEnumerable<int> values)
{
    public Partition(params int[] input) : this(input as IEnumerable<int>)
    {
    }
    public Partition(int first, IEnumerable<int> middle, int last) : this(new[] { first }.Concat(middle).Concat(new[] { last }) as IEnumerable<int>) { }
    public IList<int> Values { get; } = values.ToList();

    public int Product => Values.Aggregate(1, (res, val) => res * val);

    public override string ToString() => $"[{string.Join(", ", Values)}]";
}