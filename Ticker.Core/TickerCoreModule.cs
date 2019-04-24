namespace Ticker.Core
{
    using Autofac;

    public class TickerCoreModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<TickerService>().As<ITickerService>().SingleInstance();
            builder.RegisterType<MessageService>().As<IMessageService>().SingleInstance();
        }
    }
}