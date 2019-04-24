namespace TickerMvvmDI.Wpf
{
    using System.Windows;
    using Windows;
    using Ticker.Core;
    using Ticker.WpfShared;

    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var bootstrapper = new AutofacBootstrapperWpf<MainWindow>();
            bootstrapper.RegisterModule<TickerCoreModule>();
            bootstrapper.RegisterModule<TickerModule>();
            bootstrapper.Run();
        }
    }
}