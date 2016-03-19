using System;

namespace UsageExample
{
    public class Die : IDie
    {
        private readonly int _numberOfSides;
        private readonly IRandom _random;
        private int _value;

        public Die(int numberOfSides, IRandom random)
        {
            _numberOfSides = numberOfSides;
            _random = random;
            _value = 0;
        }

        public void Roll()
        {
            _value = _random.Next(1, _numberOfSides + 1);
        }

        public int GetValue()
        {
            return _value;
        }
    }
}