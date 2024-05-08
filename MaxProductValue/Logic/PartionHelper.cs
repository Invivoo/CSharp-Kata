

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

            //new (2, 1, 1, 1, 1, 1, 1),
            //(6, 1, 1),
            // 6, 2
            // 5, 1, 1, 1
            // 5, 2, 1
            // 4, 1, 1, 1, 1
            
            // 4, 2, 1, 1 *****
            // 4, 2, 2

            // 4, 3, 1
            // 2, 1, 1, 1, 1, 1, 1

            var nextIndex = 1;
            var second = new Partition(current.Values);
            while (second.Values.Count > nextIndex + 1 && second.Values[nextIndex] < second.Values[nextIndex - 1])
            {
                //4, 2, 1, 1
                var parameters = second.Values.Take(nextIndex)
                    .Concat(new int[] { second.Values[nextIndex] + second.Values[nextIndex + 1] })
                    .Concat(second.Values.Skip(nextIndex+2));
                second = new Partition(parameters);
                yield return second; // 4, 2, 1, 1 => 4, 3, 1


                //4, 2, 2 => 4, 3, 1 nextIndex = 2, second.Values[nextIndex] = 2
                // 3, 2, 2, 1
                var third = new Partition(second.Values);
                while (nextIndex > 1 && third.Values[nextIndex] > 1 && third.Values[nextIndex - 1] < third.Values[nextIndex - 2])
                {
                    var nextParam = third.Values.Take(nextIndex - 1)
                        .Concat(new int[] { third.Values[nextIndex - 1] +1, third.Values[nextIndex] -1 })
                        .Concat(third.Values.Skip(nextIndex + 1));
                    third = new Partition(nextParam);
                    yield return third;
                    // 3, 3, 1, 1 => recursivité? (split de third)
                }
                
                nextIndex++;
            }
        }
    }
}