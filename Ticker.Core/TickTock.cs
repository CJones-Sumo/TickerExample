namespace Ticker.Core
{
    using System;
    using System.Reactive.Linq;
    using System.Reactive.Subjects;
    using System.Threading;
    using System.Threading.Tasks;

    public class TickTock : IDisposable
    {
        private readonly Subject<int> tickSubject = new Subject<int>();
        private readonly int to;

        private readonly TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();

        public TickTock(int from, int to)
        {
            this.to = to;
            var nextValue = from;
            Timer timer = null;
            timer = new Timer(_ =>
                {
                    this.CurrentCount = nextValue;
                    nextValue++;
                    this.tickSubject.OnNext(this.CurrentCount);
                    if (this.CurrentCount < to)
                    {
                        return;
                    }

                    this.tickSubject.OnCompleted();
                    timer?.Dispose();
                    this.tcs.SetResult(true);
                },
                null,
                TimeSpan.FromSeconds(0),
                TimeSpan.FromSeconds(1));
        }

        public int CurrentCount { get; private set; } = -1;

        public IObservable<int> WhenCurrentCountChanged => this.tickSubject.AsObservable();

        public void Dispose()
        {
            this.tickSubject?.Dispose();
        }

        public async Task WaitAsync()
        {
            await this.tcs.Task;
        }
    }
}