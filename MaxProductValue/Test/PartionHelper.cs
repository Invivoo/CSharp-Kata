

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
        throw new NotImplementedException();
    }
}