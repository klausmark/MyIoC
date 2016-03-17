using System;

namespace UsageExample
{
    public class Die : IDie
    {
        private readonly IRandom _random;
        private int _value;

        public Die(IRandom random)
        {
            _random = random;
            _value = 0;
        }

        public void Roll()
        {
            _value = _random.Next(1, 7);
        }

        public int GetValue()
        {
            return _value;
        }
    }
}