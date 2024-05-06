using System;

namespace P2.Observable
{
    public interface IObservableProperty<T>
    {
        DisposableSubscription<T> OnValueChanged { get; set; }
        T Value { get; }
        IDisposable InvokeAndSubscribe(Action<T> observer);
    }
}