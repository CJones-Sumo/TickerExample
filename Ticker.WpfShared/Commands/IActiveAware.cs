namespace Ticker.WpfShared.Commands
{
    using System;

    public interface IActiveAware
    {
        bool IsActive { get; set; }

        event EventHandler IsActiveChanged;
    }
}