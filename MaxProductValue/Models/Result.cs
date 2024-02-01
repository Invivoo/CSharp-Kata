using System.Runtime.InteropServices;

public class Result
{
    public int MaxValue {get; set;}
    public List<Partition> ListPartitions = new();

    public Result(int maxValue, params Partition[] partitions)
    {
        this.MaxValue = maxValue;
        this.ListPartitions = partitions.ToList();
    }

    public Result()
    {
    }

    public override string ToString()
    {
        return $"([{string.Join(", ", ListPartitions)}], {MaxValue})";
    }
}

public class Partition
{
    public Partition(List<int> values)
    {
        this.Values = values;
    }

    public List<int> Values { get; }= [];

    public int Product => Values.Aggregate(1, (res, val) => res * val);

    public int ComputeProduct()
    {
        var result = 1;
        foreach (var v in Values)
        {
            result *= v;
        }

        return result;
    }
    
    public override string ToString()
    {
        return $"[{string.Join(", ", Values)}]";
    }
}