namespace Ticker.WpfShared
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Windows;
    using Autofac;
    using Shared;
    using Module = Autofac.Module;

    public class AutofacBootstrapperWpf<TMainWindow> : AutofacBootstrapper
        where TMainWindow : Window
    {
        public AutofacBootstrapperWpf(string jsonModulesFile = null)
            : base(jsonModulesFile)
        {
            var viewScopes = new Dictionary<FrameworkElement, ILifetimeScope>();

            ViewModelLocationProvider.SetDefaultViewModelFactory((view, type) =>
            {
                if (!(view is FrameworkElement element))
                {
                    return this.Container.Resolve(type);
                }

                if (!viewScopes.TryGetValue(element, out var scope))
                {
                    scope = this.Container.BeginLifetimeScope();
                    viewScopes[element] = scope;
                    element.Unloaded += (sender, args) =>
                    {
                        scope.Dispose();
                        viewScopes.Remove(element);
                    };
                }

                var vm = scope.Resolve(type);
                return vm;
            });

            ViewModelLocationProvider.SetDefaultViewTypeToViewModelTypeResolver(
                viewType =>
                {
                    var viewName = viewType.Name;
                    var viewAssemblyName = viewType.GetTypeInfo().Assembly.FullName;

                    var viewModelShortName = viewName;
                    if (viewModelShortName == null)
                    {
                        throw new Exception("ViewName is null");
                    }

                    if (viewModelShortName.EndsWith("View"))
                    {
                        viewModelShortName += "Model";
                    }
                    else
                    {
                        viewModelShortName += "ViewModel";
                    }

                    var viewModelName = $"{viewType.Namespace}.{viewModelShortName}";
                    var viewInterfaceName = $"{viewType.Namespace}.I{viewModelShortName}";
                    var interfaceType = viewType.Assembly.GetType($"{viewInterfaceName}");
                    return interfaceType != null
                        ? interfaceType
                        : Type.GetType(viewModelName);
                });
        }

        public void Run()
        {
            this.RegisterModule<ShellModule<TMainWindow>>();
            this.Build();
            this.InitializeShell();
        }

        protected void InitializeShell()
        {
            Application.Current.MainWindow = this.Container.Resolve<TMainWindow>();
            Application.Current.MainWindow?.Show();
            if (Application.Current?.MainWindow != null)
            {
                Application.Current.MainWindow.Closed += this.MainWindowOnClosed;
            }
        }

        private void MainWindowOnClosed(object sender, EventArgs e)
        {
            Application.Current.Shutdown(0);
        }

        private class ShellModule<TShell> : Module
        {
            protected override void Load(ContainerBuilder builder)
            {
                builder.RegisterType<TShell>();
            }
        }
    }
}