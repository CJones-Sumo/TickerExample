namespace Ticker.Core
{
    public interface ITickerService
    {
        TickTock CreateCount(int from, int to);
    }
}