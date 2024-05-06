using System;
using P2.Observable;

namespace P2.Objectives
{
    public abstract class Objective : IDisposable
    {
        public abstract event Action OnComplete;
        public abstract IObservableProperty<int> Value { get; }
        public abstract void Dispose();
    }
}