using FluentAssertions;
namespace Anagram
{
    public static class AnagramHelper
    {
        public static IList<string> Words = BuildWords();

        public static IList<string> BuildWords()
        {
            var wordsFileContent = File.ReadAllText("./words.txt");
            return wordsFileContent.Split(new[] { " ", Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
        }

        public static IEnumerable<string> GetAnagrams(string entry) => GetAnagrams(entry, Words);

        public static IEnumerable<string> GetAnagrams(string entry, IList<string> words)
        {
            var orderedEntry = new string(entry.Concat(new[] { ' ' }).OrderBy(x => x).ToArray());
            foreach (var word in Get2WordsCombination(words))
            {
                var orderedCharaters = new string(word.OrderBy(x => x).ToArray());
                if (orderedEntry == orderedCharaters)
                    yield return word;
            }
        }

        //Todo: Ne pas tenir compte de l'ordre des mots "tokyo yes" et "yes tokyo" devrait etre considéré comme la meme chose
        public static IEnumerable<string> Get2WordsCombination(IEnumerable<string> entry)
        {
            foreach(var first in entry)
            {
                foreach(var second in entry)
                {
                    if (second == first)
                        continue;
                    yield return $"{first} {second}";
                }
            }
        }
    }

    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            Assert.Multiple(
                () => Assert.Contains("acrobat", AnagramHelper.Words),
                () => Assert.Contains("tokyo", AnagramHelper.Words),
                () => Assert.Contains("yes", AnagramHelper.Words)
                );
        }

        [Fact]
        public void Test_ShouldReturnYesTokyo()
        {
            var actual = AnagramHelper.GetAnagrams("yestokyo", new[] { "yes", "tokyo" }).ToArray();
            var expected = new[] { "yes tokyo", "tokyo yes" };
            
            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void Test_ShouldReturnAllWordsCombination()
        {
            var actual = AnagramHelper.Get2WordsCombination(new[] {"yes", "tokyo", "paris"}).ToArray();
            var expected = new[] { "yes tokyo", "yes paris", "tokyo paris", "tokyo yes", "paris yes", "paris tokyo" };
            actual.Should().BeEquivalentTo(expected);
        }
    }
}