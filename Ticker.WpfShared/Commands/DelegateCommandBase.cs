namespace Ticker.WpfShared.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Windows.Input;

    public abstract class DelegateCommandBase : ICommand, IActiveAware
    {
        private readonly HashSet<string> observedPropertiesExpressions = new HashSet<string>();

        private readonly SynchronizationContext synchronizationContext;
        private bool isActive;

        protected DelegateCommandBase()
        {
            this.synchronizationContext = SynchronizationContext.Current;
        }

        public bool IsActive
        {
            get => this.isActive;
            set
            {
                if (this.isActive != value)
                {
                    this.isActive = value;
                    this.OnIsActiveChanged();
                }
            }
        }

        public virtual event EventHandler IsActiveChanged;

        public virtual event EventHandler CanExecuteChanged;

        void ICommand.Execute(object parameter)
        {
            this.Execute(parameter);
        }

        bool ICommand.CanExecute(object parameter)
        {
            return this.CanExecute(parameter);
        }

        protected virtual void OnCanExecuteChanged()
        {
            var handler = this.CanExecuteChanged;
            if (handler != null)
            {
                if (this.synchronizationContext != null &&
                    this.synchronizationContext != SynchronizationContext.Current)
                {
                    this.synchronizationContext.Post(o => handler.Invoke(this, EventArgs.Empty), null);
                }
                else
                {
                    handler.Invoke(this, EventArgs.Empty);
                }
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")]
        public void RaiseCanExecuteChanged()
        {
            this.OnCanExecuteChanged();
        }

        protected abstract void Execute(object parameter);

        protected abstract bool CanExecute(object parameter);

        protected internal void ObservesPropertyInternal<T>(Expression<Func<T>> propertyExpression)
        {
            if (this.observedPropertiesExpressions.Contains(propertyExpression.ToString()))
            {
                throw new ArgumentException($"{propertyExpression} is already being observed.",
                    nameof(propertyExpression));
            }

            this.observedPropertiesExpressions.Add(propertyExpression.ToString());
            PropertyObserver.Observes(propertyExpression, this.RaiseCanExecuteChanged);
        }

        protected virtual void OnIsActiveChanged()
        {
            this.IsActiveChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}