using System;

namespace P2.Observable
{
    public class Unsubscriber<T> : IDisposable
    {
        private readonly Action _unsubscribeAction;

        public Unsubscriber(Action<T> subscribeAction, Action<T> action)
        {
            _unsubscribeAction = () => subscribeAction -= action;
        }

        public void Dispose()
        {
            _unsubscribeAction?.Invoke();
        }
    }
}