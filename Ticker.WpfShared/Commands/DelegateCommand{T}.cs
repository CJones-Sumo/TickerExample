namespace Ticker.WpfShared.Commands
{
    using System;
    using System.Linq.Expressions;
    using System.Reflection;

    public class DelegateCommand<T> : DelegateCommandBase
    {
        private readonly Action<T> executeMethod;
        private Func<T, bool> canExecuteMethod;

        public DelegateCommand(Action<T> executeMethod)
            : this(executeMethod, o => true)
        {
        }

        public DelegateCommand(Action<T> executeMethod, Func<T, bool> canExecuteMethod)
        {
            if (executeMethod == null || canExecuteMethod == null)
            {
                throw new ArgumentNullException(nameof(executeMethod));
            }

            var genericTypeInfo = typeof(T).GetTypeInfo();

            // DelegateCommand allows object or Nullable<>.  
            // note: Nullable<> is a struct so we cannot use a class constraint.
            if (genericTypeInfo.IsValueType)
            {
                if (!genericTypeInfo.IsGenericType || !typeof(Nullable<>)
                                                       .GetTypeInfo()
                                                       .IsAssignableFrom(genericTypeInfo
                                                                         .GetGenericTypeDefinition()
                                                                         .GetTypeInfo()))
                {
                    throw new InvalidCastException();
                }
            }

            this.executeMethod = executeMethod;
            this.canExecuteMethod = canExecuteMethod;
        }

        public void Execute(T parameter)
        {
            this.executeMethod(parameter);
        }

        public bool CanExecute(T parameter)
        {
            return this.canExecuteMethod(parameter);
        }

        protected override void Execute(object parameter)
        {
            this.Execute((T)parameter);
        }

        protected override bool CanExecute(object parameter)
        {
            return this.CanExecute((T)parameter);
        }

        public DelegateCommand<T> ObservesProperty<TType>(Expression<Func<TType>> propertyExpression)
        {
            this.ObservesPropertyInternal(propertyExpression);
            return this;
        }

        public DelegateCommand<T> ObservesCanExecute(Expression<Func<bool>> canExecuteExpression)
        {
            var expression =
                Expression.Lambda<Func<T, bool>>(canExecuteExpression.Body, Expression.Parameter(typeof(T), "o"));
            this.canExecuteMethod = expression.Compile();
            this.ObservesPropertyInternal(canExecuteExpression);
            return this;
        }
    }
}