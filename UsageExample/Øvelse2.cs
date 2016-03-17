namespace UsageExample
{
    public class Øvelse2
    {
        private readonly IØvelsesLogger _logger;
        private readonly IDieRoller _roller;

        public Øvelse2(IØvelsesLogger logger, IDieRoller roller)
        {
            _logger = logger;
            _roller = roller;
        }

        public void GoAhead()
        {
            _roller.Roll();
            _logger.Log(_roller.ToString());
        }
    }
}