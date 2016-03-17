namespace UsageExample
{
    public class DieCup : IDieRoller
    {
        private readonly IDie _die1;
        private readonly IDie _die2;

        public DieCup(IDie die1, IDie die2)
        {
            _die1 = die1;
            _die2 = die2;
        }

        public void Roll()
        {
            _die1.Roll();
            _die2.Roll();
        }

        public int GetValue()
        {
            return _die1.GetValue() + _die2.GetValue();
        }

        public override string ToString()
        {
            return $"{_die1.GetValue()} + {_die2.GetValue()} = {GetValue()}";
        }
    }
}