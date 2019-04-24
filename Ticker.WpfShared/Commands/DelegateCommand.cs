namespace Ticker.WpfShared.Commands
{
    using System;
    using System.Linq.Expressions;

    public class DelegateCommand : DelegateCommandBase
    {
        private readonly Action executeMethod;
        private Func<bool> canExecuteMethod;

        public DelegateCommand(Action executeMethod)
            : this(executeMethod, () => true)
        {
        }

        public DelegateCommand(Action executeMethod, Func<bool> canExecuteMethod)
        {
            if (executeMethod == null || canExecuteMethod == null)
            {
                throw new ArgumentNullException(nameof(executeMethod));
            }

            this.executeMethod = executeMethod;
            this.canExecuteMethod = canExecuteMethod;
        }

        public void Execute()
        {
            this.executeMethod();
        }

        public bool CanExecute()
        {
            return this.canExecuteMethod();
        }

        protected override void Execute(object parameter)
        {
            this.Execute();
        }

        protected override bool CanExecute(object parameter)
        {
            return this.CanExecute();
        }

        public DelegateCommand ObservesProperty<T>(Expression<Func<T>> propertyExpression)
        {
            this.ObservesPropertyInternal(propertyExpression);
            return this;
        }

        public DelegateCommand ObservesCanExecute(Expression<Func<bool>> canExecuteExpression)
        {
            this.canExecuteMethod = canExecuteExpression.Compile();
            this.ObservesPropertyInternal(canExecuteExpression);
            return this;
        }
    }
}