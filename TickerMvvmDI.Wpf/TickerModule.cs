namespace TickerMvvmDI.Wpf
{
    using Windows;
    using Autofac;
    using Controls.EntryArea;
    using Controls.MessagePanel;
    using Controls.NotificationBar;

    internal class TickerModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<MainWindowViewModel>().As<IMainWindowViewModel>();
            builder.RegisterType<NotificationBarViewModel>().As<INotificationBarViewModel>();
            builder.RegisterType<EntryAreaViewModel>().As<IEntryAreaViewModel>();
            builder.RegisterType<MessagePanelViewModel>().As<IMessagePanelViewModel>();
        }
    }
}