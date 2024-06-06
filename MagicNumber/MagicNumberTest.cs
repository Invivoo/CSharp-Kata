using Microsoft.Extensions.Caching.Memory;
namespace MagicNumber
{
    public class MagicNumberTest
    {
        private IMemoryCache _cache;

        public MagicNumberTest()
        {
            _cache = new Microsoft.Extensions.Caching.Memory.MemoryCache(new MemoryCacheOptions());
        }
        [Fact]
        public void Test1()
        {
            var result = CalculateMagicNumber(0);
            int expected = 0;
            Assert.Equal(expected, result);

            result = CalculateMagicNumber(1);
            expected = 2;
            Assert.Equal(expected, result);

            result = CalculateMagicNumber(10000);
            expected = 100010000;
            Assert.Equal(expected, result);

            result = CalculateMagicNumber(100000000);
            expected = 100010000;
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Test2()
        {
            var result = CalculateMagicNumberV2(0);
            int expected = 0;
            Assert.Equal(expected, result);

            result = CalculateMagicNumberV2(1);
            expected = 2;
            Assert.Equal(expected, result);

            result = CalculateMagicNumberV2(10000);
            expected = 100010000;
            Assert.Equal(expected, result);

            result = CalculateMagicNumberV2(100000000);
            expected = 1974919424;
            Assert.Equal(expected, result);

            result = CalculateMagicNumberV2(1000000000);
            expected = -486618624;
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Test3()
        {
            var result = CalculateMagicNumberV3(0);
            long expected = 0;
            Assert.Equal(expected, result);

            result = CalculateMagicNumberV3(1);
            expected = 2;
            Assert.Equal(expected, result);

            result = CalculateMagicNumberV3(10000);
            expected = 100010000;
            Assert.Equal(expected, result);

            result = CalculateMagicNumberV3(100000000);
            expected = 10000000100000000;
            Assert.Equal(expected, result);

            result = CalculateMagicNumberV3(1000000000);
            expected = 1000000001000000000;
            Assert.Equal(expected, result);

            result = CalculateMagicNumberV3(1000000000);
            expected = 1000000001000000000;
            Assert.Equal(expected, result);

            result = CalculateMagicNumberV3(1000000000);
            expected = 1000000001000000000;
            Assert.Equal(expected, result);

            result = CalculateMagicNumberV3(1000000000);
            expected = 1000000001000000000;
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Test4()
        {
            var result = CheckNumberInCache(0);
            long expected = 0;
            Assert.Equal(expected, result);

            result = CheckNumberInCache(1);
            expected = 2;
            Assert.Equal(expected, result);

            result = CheckNumberInCache(10000);
            expected = 100010000;
            Assert.Equal(expected, result);

            result = CheckNumberInCache(100000000);
            expected = 10000000100000000;
            Assert.Equal(expected, result);

            result = CheckNumberInCache(1000000000);
            expected = 1000000001000000000;
            Assert.Equal(expected, result);

            result = CheckNumberInCache(1000000000);
            expected = 1000000001000000000;
            Assert.Equal(expected, result);

            result = CheckNumberInCache(1000000000);
            expected = 1000000001000000000;
            Assert.Equal(expected, result);

            result = CheckNumberInCache(1000000005);
            expected = 1000000015000000028;
            Assert.Equal(expected, result);
        }
        public int CalculateMagicNumber(int n)
        {
            if (n == 0)
                return 0;
            return 2 * n + CalculateMagicNumber(n - 1);
        }
        public int CalculateMagicNumberV2(int n)
        {
            if (n == 0)
                return 0;
            int sum = 0;
            for (int i = n; i >= 1; i--)
            {
                sum += 2 * i;
            }
            return sum;
        }
        public long CalculateMagicNumberV3(int n)
        {
            if (_cache.TryGetValue(n, out long cacheResult))
                return cacheResult;
            if (n == 0)
                return 0;
            long sum = 0;
            for (int i = n; i >= 1; i--)
            {
                sum += 2 * i;
            }
            _cache.Set(n, sum);
            return sum;
        }
        /*
         cache(1000)
        
        n = 1005
         return 5
        cache(1000)
         */
        public long CheckNumberInCache(int n)
        {
            if (_cache.TryGetValue(n, out long cacheResult))
                return cacheResult;
            int current = n;
            int diff = 0;
            long cacheResultSave = 0;
            while (!_cache.TryGetValue(current--, out cacheResultSave) && diff < 10)
            {
                diff++;
            }
            if (cacheResultSave == 0)
                return CalculateMagicNumberV3(n);
            return CalculateMagicNumberV4(n, cacheResultSave, current);
        }
        public long CalculateMagicNumberV4(int n, long from, int fromIndex)
        {
            if (_cache.TryGetValue(n, out long cacheResult))
                return cacheResult;
            if (n == 0)
                return 0;
            long sum = from;
            for (int i = n; i >= fromIndex; i--)
            {
                sum += 2 * i;
            }
            _cache.Set(n, sum);
            return sum;
        }
    }
}