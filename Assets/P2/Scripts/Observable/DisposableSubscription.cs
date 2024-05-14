using System;

namespace P2.Observable
{
    public class DisposableSubscription<T> : IDisposable
    {
        private readonly IObservableProperty<T> _property;
        private event Action<T> SubscribeAction;

        private readonly CompositeDisposable _disposables = new();

        public DisposableSubscription(IObservableProperty<T> property)
        {
            _property = property;
        }

        public void Invoke(T value)
        {
            SubscribeAction?.Invoke(value);
        }

        public IDisposable Subscribe(Action<T> action)
        {
            SubscribeAction += action;
            return _disposables.Add(new Unsubscriber<T>(SubscribeAction, action));
        }

        public void Dispose()
        {
            _disposables?.Dispose();
        }
    }
}