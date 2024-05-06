using P2.Observable;
using UnityEngine;

namespace P2.UI
{
    public abstract class ViewModel
    {
        public abstract void Show();
    }

    public abstract class ViewModel<T> : ViewModel where T : View
    {
        protected T view;
        protected readonly CompositeDisposable disposables = new();

        public sealed override void Show()
        {
            var prefab = Resources.Load<T>(typeof(T).Name);
            view = Object.Instantiate(prefab);
            ShowInternal();
        }

        public void Close()
        {
            disposables.Dispose();
            view.Dispose();
        }

        protected abstract void ShowInternal();
    }
}