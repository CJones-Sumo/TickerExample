namespace Ticker.Core
{
    using System;
    using System.Collections.Generic;
    using System.Reactive.Linq;
    using System.Reactive.Subjects;

    internal class MessageService : IMessageService
    {
        private readonly Queue<string> messageQueue;
        private readonly Subject<string> messageSubject;

        public MessageService()
        {
            this.messageSubject = new Subject<string>();
            this.messageQueue = new Queue<string>();
        }

        public IObservable<string> WhenMessageEnqueued => this.messageSubject.AsObservable();

        public IEnumerable<string> Messages => this.messageQueue;

        public void EnqueueMessage(string message)
        {
            this.messageQueue.Enqueue(message);
            this.messageSubject.OnNext(message);
        }
    }
}