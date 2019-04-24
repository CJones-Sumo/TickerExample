namespace TickerMvvmDI.Wpf.Controls.EntryArea
{
    using System.Windows.Input;
    using Ticker.Core;
    using Ticker.WpfShared;
    using Ticker.WpfShared.Commands;

    internal class EntryAreaViewModel : BindableBase, IEntryAreaViewModel
    {
        private readonly DelegateCommand enterMessageCommand;
        private readonly IMessageService messageService;
        private string message;

        public EntryAreaViewModel(IMessageService messageService)
        {
            this.messageService = messageService;
            this.enterMessageCommand = new DelegateCommand(
                this.ExecuteEnterMessage,
                this.CanExecuteEnterMessage);
        }

        public ICommand EnterMessageCommand => this.enterMessageCommand;

        public string Message
        {
            get => this.message;
            set => this.SetProperty(ref this.message,
                value,
                () => { this.enterMessageCommand.RaiseCanExecuteChanged(); });
        }

        private bool CanExecuteEnterMessage()
        {
            return !string.IsNullOrWhiteSpace(this.Message);
        }

        private void ExecuteEnterMessage()
        {
            this.messageService.EnqueueMessage(this.Message);
            this.Message = string.Empty;
        }
    }
}