public class MaxProductValue
{
    public int input { get; set; }
    public Result? result { get; set; }

    public Result GetMaxProductValue(int input)
    {
        var listPartition = new Partition(new List<int>() { input });
        var initialPartition = new List<int>() { input};
        for(int i = input; i > 0; i--)
        {
            var nextPartition = GetNextPartition(input, initialPartition);
            
        }
        return new Result();
    }
    public List<int> GetNextPartition(int input, List<int> partition)
    {
        return new List<int>();
    }
}