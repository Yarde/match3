using System;
using P2.Observable;
using TMPro;

namespace P2.UI
{
    public static class BindingExtensions
    {
        public static IDisposable Bind(this TextMeshProUGUI text, IObservableProperty<int> observableProperty, string format = "{0}")
        {
            return observableProperty.InvokeAndSubscribe(value => text.text = string.Format(format, value));
        }
        
        public static void AddTo(this IDisposable disposable, CompositeDisposable disposables)
        {
            disposables.Add(disposable);
        }
    }
}