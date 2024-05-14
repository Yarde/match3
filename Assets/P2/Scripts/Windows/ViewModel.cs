using P2.Observable;
using UnityEngine;

namespace P2.Windows
{
    public abstract class ViewModel
    {
        public abstract void Setup();
        public abstract void Show();
        public abstract void Hide();
        public abstract void Close();
    }

    public abstract class ViewModel<T> : ViewModel where T : View
    {
        protected T view;
        protected readonly CompositeDisposable disposables = new();

        public sealed override void Setup()
        {
            var prefab = Resources.Load<T>(typeof(T).Name);
            view = Object.Instantiate(prefab);
            SetupInternal();
        }

        public override void Show()
        {
            view.gameObject.SetActive(true);
        }

        public override void Hide()
        {
            view.gameObject.SetActive(false);
        }

        public override void Close()
        {
            disposables.Dispose();
            view.Dispose();
        }

        protected abstract void SetupInternal();
    }
}