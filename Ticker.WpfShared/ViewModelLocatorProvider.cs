// From: https://github.com/PrismLibrary/Prism

namespace Ticker.WpfShared
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.Reflection;

    public static class ViewModelLocationProvider
    {
        private static readonly Dictionary<string, Func<object>> Factories = new Dictionary<string, Func<object>>();

        private static readonly Dictionary<string, Type> TypeFactories = new Dictionary<string, Type>();

        private static Func<Type, object> DefaultViewModelFactory = Activator.CreateInstance;

        private static Func<object, Type, object> DefaultViewModelFactoryWithViewParameter;

        private static Func<Type, Type> DefaultViewTypeToViewModelTypeResolver =
            viewType =>
            {
                var viewName = viewType.FullName;
                Debug.Assert(viewName != null, nameof(viewName) + " != null");
                viewName = viewName.Replace(".Views.", ".ViewModels.");
                var viewAssemblyName = viewType.GetTypeInfo().Assembly.FullName;
                var suffix = viewName.EndsWith("View")
                    ? "Model"
                    : "ViewModel";
                var viewModelName = string.Format(CultureInfo.InvariantCulture,
                    "{0}{1}, {2}",
                    viewName,
                    suffix,
                    viewAssemblyName);
                return Type.GetType(viewModelName);
            };

        /// <summary>
        ///     Sets the default view model factory.
        /// </summary>
        /// <param name="viewModelFactory">The view model factory which provides the ViewModel type as a parameter.</param>
        public static void SetDefaultViewModelFactory(Func<Type, object> viewModelFactory)
        {
            DefaultViewModelFactory = viewModelFactory;
        }

        /// <summary>
        ///     Sets the default view model factory.
        /// </summary>
        /// <param name="viewModelFactory">The view model factory that provides the View instance and ViewModel type as parameters.</param>
        public static void SetDefaultViewModelFactory(Func<object, Type, object> viewModelFactory)
        {
            DefaultViewModelFactoryWithViewParameter = viewModelFactory;
        }

        /// <summary>
        ///     Sets the default view type to view model type resolver.
        /// </summary>
        /// <param name="viewTypeToViewModelTypeResolver">The view type to view model type resolver.</param>
        public static void SetDefaultViewTypeToViewModelTypeResolver(Func<Type, Type> viewTypeToViewModelTypeResolver)
        {
            DefaultViewTypeToViewModelTypeResolver = viewTypeToViewModelTypeResolver;
        }

        /// <summary>
        ///     Automatically looks up the viewmodel that corresponds to the current view, using two strategies:
        ///     It first looks to see if there is a mapping registered for that view, if not it will fallback to the convention
        ///     based approach.
        /// </summary>
        /// <param name="view">The dependency object, typically a view.</param>
        /// <param name="setDataContextCallback">The call back to use to create the binding between the View and ViewModel</param>
        public static void AutoWireViewModelChanged(object view, Action<object, object> setDataContextCallback)
        {
            // Try mappings first
            var viewModel = GetViewModelForView(view);

            // try to use ViewModel type
            if (viewModel == null)
            {
                //check type mappings
                var viewModelType = GetViewModelTypeForView(view.GetType());

                // fallback to convention based
                if (viewModelType == null)
                {
                    viewModelType = DefaultViewTypeToViewModelTypeResolver(view.GetType());
                }

                if (viewModelType == null)
                {
                    return;
                }

                viewModel = DefaultViewModelFactoryWithViewParameter != null
                    ? DefaultViewModelFactoryWithViewParameter(view, viewModelType)
                    : DefaultViewModelFactory(viewModelType);
            }


            setDataContextCallback(view, viewModel);
        }

        /// <summary>
        ///     Gets the view model for the specified view.
        /// </summary>
        /// <param name="view">The view that the view model wants.</param>
        /// <returns>The ViewModel that corresponds to the view passed as a parameter.</returns>
        private static object GetViewModelForView(object view)
        {
            var viewKey = view.GetType().ToString();

            // Mapping of view models base on view type (or instance) goes here
            if (Factories.ContainsKey(viewKey))
            {
                return Factories[viewKey]();
            }

            return null;
        }

        /// <summary>
        ///     Gets the ViewModel type for the specified view.
        /// </summary>
        /// <param name="view">The View that the ViewModel wants.</param>
        /// <returns>The ViewModel type that corresponds to the View.</returns>
        private static Type GetViewModelTypeForView(Type view)
        {
            var viewKey = view.ToString();

            if (TypeFactories.ContainsKey(viewKey))
            {
                return TypeFactories[viewKey];
            }

            return null;
        }

        /// <summary>
        ///     Registers the ViewModel factory for the specified view type.
        /// </summary>
        /// <typeparam name="T">The View</typeparam>
        /// <param name="factory">The ViewModel factory.</param>
        public static void Register<T>(Func<object> factory)
        {
            Register(typeof(T).ToString(), factory);
        }

        /// <summary>
        ///     Registers the ViewModel factory for the specified view type name.
        /// </summary>
        /// <param name="viewTypeName">The name of the view type.</param>
        /// <param name="factory">The ViewModel factory.</param>
        public static void Register(string viewTypeName, Func<object> factory)
        {
            Factories[viewTypeName] = factory;
        }

        /// <summary>
        ///     Registers a ViewModel type for the specified view type.
        /// </summary>
        /// <typeparam name="T">The View</typeparam>
        /// <typeparam name="TVm">The ViewModel</typeparam>
        public static void Register<T, TVm>()
        {
            var viewType = typeof(T);
            var viewModelType = typeof(TVm);

            Register(viewType.ToString(), viewModelType);
        }

        /// <summary>
        ///     Registers a ViewModel type for the specified view.
        /// </summary>
        /// <param name="viewTypeName">The View type name</param>
        /// <param name="viewModelType">The ViewModel type</param>
        public static void Register(string viewTypeName, Type viewModelType)
        {
            TypeFactories[viewTypeName] = viewModelType;
        }
    }
}