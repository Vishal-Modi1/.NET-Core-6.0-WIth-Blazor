using System;
using System.Text;

namespace GlobalUtilities
{
    public class RandomTextGenerator
    {
        private readonly Random _random;

        public RandomTextGenerator()
        {
            _random = new Random();
        }

        public string Generate()
        {
            var builder = new StringBuilder(200);

            char offset = 'a';
            const int lettersOffset = 26; 

            for (var i = 0; i < 200; i++)
            {
                char text = (char)_random.Next(offset, offset + lettersOffset);
                builder.Append(text);
            }

            return  builder.ToString().ToLower() ;
        }
    }
}
