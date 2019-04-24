namespace TickerMvvmDI.Wpf.Controls.EntryArea
{
    using System.Windows.Input;

    internal interface IEntryAreaViewModel
    {
        ICommand EnterMessageCommand { get; }

        string Message { get; set; }
    }
}