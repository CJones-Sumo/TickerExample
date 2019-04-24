namespace TickerMvvmDI.Wpf.Controls.NotificationBar
{
    using System;
    using System.Reactive.Linq;
    using System.Threading.Tasks;
    using Ticker.Core;
    using Ticker.WpfShared;

    internal class NotificationBarViewModel : BindableBase, INotificationBarViewModel
    {
        private string currentMessage;

        public NotificationBarViewModel(ITickerService tickerService)
        {
            Task.Run(async () =>
            {
                using (var ticker = tickerService.CreateCount(0, 360))
                {
                    ticker.WhenCurrentCountChanged.StartWith()
                          .Subscribe(count => { this.CurrentMessage = $"Count: {count}"; });

                    await ticker.WaitAsync();
                }
            });
        }

        public string CurrentMessage
        {
            get => this.currentMessage;
            set => this.SetProperty(ref this.currentMessage, value);
        }
    }
}