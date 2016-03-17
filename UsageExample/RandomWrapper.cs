using System;

namespace UsageExample
{
    public class RandomWrapper : IRandom
    {
        private readonly Random _random;

        public RandomWrapper()
        {
            _random = new Random();
        }

        public RandomWrapper(int seed)
        {
            _random = new Random(seed);
        }

        public int Next(int minValue, int maxValue)
        {
            return _random.Next(minValue, maxValue);
        }
    }
}