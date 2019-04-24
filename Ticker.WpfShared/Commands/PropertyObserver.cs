namespace Ticker.WpfShared.Commands
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq.Expressions;
    using System.Reflection;

    internal class PropertyObserver
    {
        private readonly Action action;

        private PropertyObserver(Expression propertyExpression, Action action)
        {
            this.action = action;
            this.SubscribeListeners(propertyExpression);
        }

        private void SubscribeListeners(Expression propertyExpression)
        {
            var propNameStack = new Stack<PropertyInfo>();
            while (propertyExpression is MemberExpression temp)
            {
                propertyExpression = temp.Expression;
                propNameStack.Push(temp.Member as PropertyInfo);
            }

            if (!(propertyExpression is ConstantExpression constantExpression))
            {
                throw new NotSupportedException("Operation not supported for the given expression type. " +
                                                "Only MemberExpression and ConstantExpression are currently supported.");
            }

            var propObserverNodeRoot = new PropertyObserverNode(propNameStack.Pop(), this.action);
            var previousNode = propObserverNodeRoot;
            foreach (var propName in propNameStack)
            {
                var currentNode = new PropertyObserverNode(propName, this.action);
                previousNode.Next = currentNode;
                previousNode = currentNode;
            }

            var propOwnerObject = constantExpression.Value;

            if (!(propOwnerObject is INotifyPropertyChanged inpcObject))
            {
                throw new InvalidOperationException("Trying to subscribe PropertyChanged listener in object that " +
                                                    $"owns '{propObserverNodeRoot.PropertyInfo.Name}' property, but the object does not implements INotifyPropertyChanged.");
            }

            propObserverNodeRoot.SubscribeListenerFor(inpcObject);
        }

        internal static PropertyObserver Observes<T>(Expression<Func<T>> propertyExpression, Action action)
        {
            return new PropertyObserver(propertyExpression.Body, action);
        }
    }
}