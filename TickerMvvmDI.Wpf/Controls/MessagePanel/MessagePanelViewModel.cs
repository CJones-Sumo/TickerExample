namespace TickerMvvmDI.Wpf.Controls.MessagePanel
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Reactive.Linq;
    using Ticker.Core;
    using Ticker.WpfShared;

    internal class MessagePanelViewModel : BindableBase, IMessagePanelViewModel
    {
        private readonly ObservableCollection<string> messages;

        public MessagePanelViewModel(IMessageService messageService)
        {
            this.messages = new ObservableCollection<string>();
            messageService.WhenMessageEnqueued
                          .StartWith(messageService.Messages)
                          .Subscribe(msg =>
                          {
                              this.messages.Add(msg);
                          });
        }

        public IEnumerable<string> Messages => this.messages;
    }
}