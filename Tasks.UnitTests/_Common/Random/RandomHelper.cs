using System;
using System.Linq;

namespace Tasks.UnitTests._Common.Random
{
    public class RandomHelper
    {
        const string NUMBERS = "0123456789";
        const string LOWERCASE_LETTERS = "abcdefghijklmnopqrstuvxwyz";
        const string UPPERCASE_LETTERS = "ABCDEFGHIJKLMNOPQRSTUVXWYZ";

        public static string RandomNumbers(int length = 10) { 
            return RandomString(length, NUMBERS);
        }

        public static string RandomString(int length = 10, string chars = NUMBERS + LOWERCASE_LETTERS + UPPERCASE_LETTERS)
        {
            if (string.IsNullOrWhiteSpace(chars))
                chars = NUMBERS + UPPERCASE_LETTERS + LOWERCASE_LETTERS;
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[StaticRandom.Next(0, s.Length)])
                .ToArray());
        }
    }

    public class StaticRandom
    {
        static int seed = Environment.TickCount;
        static readonly System.Threading.ThreadLocal<System.Random> random = new System.Threading.ThreadLocal<System.Random>(() => new System.Random(System.Threading.Interlocked.Increment(ref seed)));

        public static int Next(int min, int max)
        {
            return random.Value.Next(min, max);
        }
        public static int Next(int max)
        {
            return random.Value.Next(max);
        }
        public static int Next()
        {
            return random.Value.Next();
        }
    }
}
