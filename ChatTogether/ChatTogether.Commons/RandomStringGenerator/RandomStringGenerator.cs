using Microsoft.Extensions.Options;
using System;
using System.Text;

namespace ChatTogether.Commons.RandomStringGenerator
{
    public class RandomStringGenerator : IRandomStringGenerator
    {
        private readonly RandomStringGeneratorConfiguration randomStringGeneratorConfiguration;

        public RandomStringGenerator(IOptions<RandomStringGeneratorConfiguration> randomStringGeneratorConfiguration)
        {
            this.randomStringGeneratorConfiguration = randomStringGeneratorConfiguration.Value;
        }

        public string Generate(RandomStringType randomStringType)
        {
            int min = 0;
            int max = 0;

            switch(randomStringType)
            {
                case RandomStringType.Token:
                    min = randomStringGeneratorConfiguration.Token.MinLength;
                    max = randomStringGeneratorConfiguration.Token.MaxLength;
                    break;

                case RandomStringType.Path:
                    min = randomStringGeneratorConfiguration.Path.MinLength;
                    max = randomStringGeneratorConfiguration.Path.MaxLength;
                    break;
            }

            Random rand = new Random((int)DateTime.Now.Ticks);
            int length = rand.Next(min, max);
            StringBuilder stringBuilder = new StringBuilder(string.Empty);

            for(int i = 0; i < length; i++)
            {
                int charId = rand.Next(0, randomStringGeneratorConfiguration.Chars.Length);
                stringBuilder.Append(randomStringGeneratorConfiguration.Chars[charId]);
            }

            return stringBuilder.ToString();
        }
    }
}
