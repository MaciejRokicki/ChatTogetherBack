using ChatTogether.Commons.RandomStringGenerator.Configurations;

namespace ChatTogether.Commons.RandomStringGenerator
{
    public class RandomStringGeneratorConfiguration
    {
        public string Chars { get; set; }
        public RandomStringTokenConfiguration Token { get; set; }
        public RandomStringPathConfiguration Path { get; set; }
    }
}
