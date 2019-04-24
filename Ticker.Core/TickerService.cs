namespace Ticker.Core
{
    internal class TickerService : ITickerService
    {
        public TickTock CreateCount(int from, int to)
        {
            return new TickTock(from, to);
        }
    }
}