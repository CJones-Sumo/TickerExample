namespace Ticker.Core
{
    using System;
    using System.Collections.Generic;

    public interface IMessageService
    {
        IObservable<string> WhenMessageEnqueued { get; }

        IEnumerable<string> Messages { get; }

        void EnqueueMessage(string message);
    }
}