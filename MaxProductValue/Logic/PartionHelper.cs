

using System.Diagnostics;

internal class PartionHelper
{
    internal static IEnumerable<Partition> GetHighestProduct(IEnumerable<Partition> parameter)
    {
        var result = new List<Partition>();
        var maxProduct = 0;
        foreach (var partition in parameter)
        {
            if (partition.Product < maxProduct)
            {
                continue;
            }
            if (partition.Product > maxProduct)
            {
                maxProduct = partition.Product;
                result = new List<Partition>();
            }
            result.Add(partition);
        }
        return result;
    }

    internal static IEnumerable<Partition> CreatePartitions(int inputValue)
    {
        var first = new Partition(inputValue);
        yield return first;
        foreach(var split in Split(first))
            yield return split;
    }

    internal static IEnumerable<Partition> Split(Partition partition)
    {
        var current = partition;
        while (current.Values.First() > 1)
        {
            Debug.WriteLine(current);
            Console.WriteLine(current);
            current = new Partition(current.Values.First() -1, current.Values.Skip(1).Take(current.Values.Count() - 1), 1);
            yield return current;
        }
    }
}