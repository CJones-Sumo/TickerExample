namespace TickerMvvmDI.Wpf.Controls.MessagePanel
{
    using System.Collections.Generic;

    public interface IMessagePanelViewModel
    {
        IEnumerable<string> Messages { get; }
    }
}