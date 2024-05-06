using System;

namespace P2.Observable
{
    public class DisposableSubscription<T> : IDisposable
    {
        private readonly IObservableProperty<T> _property;
        private readonly Action<T> _subscribeAction;

        private readonly CompositeDisposable _disposables = new();
        
        public DisposableSubscription(IObservableProperty<T> property, Action<T> action)
        {
            _property = property;
            _subscribeAction = action;
        }
        
        public void Invoke(T value)
        {
            _subscribeAction?.Invoke(value);
        }
        
        public IDisposable Subscribe(Action<T> action)
        {
            action.Invoke(_property.Value);
            return _disposables.Add(new Unsubscriber<T>(_subscribeAction, action));
        }

        public void Dispose()
        {
            _disposables?.Dispose();
        }
    }
}