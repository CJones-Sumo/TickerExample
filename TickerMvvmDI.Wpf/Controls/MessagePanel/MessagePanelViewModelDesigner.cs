namespace TickerMvvmDI.Wpf.Controls.MessagePanel
{
    using System.Collections.Generic;

    internal class MessagePanelViewModelDesigner : IMessagePanelViewModel
    {
        public IEnumerable<string> Messages { get; } = new List<string>
        {
            "Test1",
            "Test2",
            "Test3"
        };
    }
}