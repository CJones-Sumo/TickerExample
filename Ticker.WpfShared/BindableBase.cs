namespace Ticker.WpfShared
{
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    public abstract class BindableBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.InvokePropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        protected virtual bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (storage?.Equals(value) ?? value == null)
            {
                return false;
            }

            storage = value;
            this.RaisePropertyChanged(propertyName);
            return true;
        }

        protected virtual bool SetProperty<T>(ref T storage, T value, Action onChanged,
                                              [CallerMemberName] string propertyName = null)
        {
            if (!this.SetProperty(ref storage, value, propertyName))
            {
                return false;
            }

            onChanged();
            return true;

        }

        private void InvokePropertyChanged(PropertyChangedEventArgs e)
        {
            this.PropertyChanged?.Invoke(this, e);
        }
    }
}