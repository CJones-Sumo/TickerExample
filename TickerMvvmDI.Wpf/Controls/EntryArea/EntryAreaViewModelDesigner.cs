namespace TickerMvvmDI.Wpf.Controls.EntryArea
{
    using System.Windows.Input;

    internal class EntryAreaViewModelDesigner : IEntryAreaViewModel
    {
        public ICommand EnterMessageCommand { get; } = null;

        public string Message { get; set; } = "Hello World";
    }
}