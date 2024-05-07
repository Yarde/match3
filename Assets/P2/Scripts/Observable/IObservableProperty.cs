using System;

namespace P2.Observable
{
    public interface IObservableProperty<T>
    {
        T PreviousValue { get; }
        T Value { get; }
        IDisposable Subscribe(Action<T> observer);
        IDisposable InvokeAndSubscribe(Action<T> observer);
    }
}