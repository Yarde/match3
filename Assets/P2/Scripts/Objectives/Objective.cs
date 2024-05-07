using System;
using P2.Observable;

namespace P2.Objectives
{
    public abstract class Objective : IDisposable
    {
        public abstract DisposableSubscription<int> OnComplete { get; }
        public abstract IObservableProperty<int> Value { get; }
        public abstract void Dispose();
    }
}