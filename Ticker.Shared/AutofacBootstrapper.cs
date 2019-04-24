namespace Ticker.Shared
{
    using Autofac;
    using Autofac.Configuration;
    using Autofac.Core;
    using Microsoft.Extensions.Configuration;

    public class AutofacBootstrapper
    {
        private readonly ConfigurationModule configModule;

        private readonly ContainerBuilder builder;

        public AutofacBootstrapper(string jsonModulesFile = null)
        {
            var configBuilder = new ConfigurationBuilder();
            if (jsonModulesFile != null)
            {
                configBuilder.AddJsonFile(jsonModulesFile);
            }

            this.configModule = new ConfigurationModule(configBuilder.Build());

            this.builder = new ContainerBuilder();
            this.builder.RegisterModule(this.configModule);
        }

        protected IContainer Container { get; private set; }

        public void RegisterModule<TModule>()
            where TModule : IModule, new()
        {
            this.builder.RegisterModule<TModule>();
        }

        public virtual void Build()
        {
            this.Container = this.builder.Build();
        }

        public T Resolve<T>()
        {
            return this.Container.Resolve<T>();
        }
    }
}