using System;

namespace P2.Objectives
{
    public abstract class Objective : IDisposable
    {
        public abstract event Action OnComplete;
        public abstract void Dispose();
    }
}