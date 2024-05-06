using System;
using System.Collections.Generic;

namespace P2.Observable
{
    public class CompositeDisposable : IDisposable
    {
        private readonly List<IDisposable> _disposables = new();
        
        public IDisposable Add(IDisposable disposable)
        {
            _disposables.Add(disposable);
            return disposable;
        }

        public void Dispose()
        {
            foreach (var disposable in _disposables)
            {
                disposable.Dispose();
            }
        }
    }
}