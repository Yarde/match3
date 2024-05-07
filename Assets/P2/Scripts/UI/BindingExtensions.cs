using System;
using P2.Levels;
using P2.Observable;
using TMPro;
using UnityEngine;

namespace P2.UI
{
    public static class BindingExtensions
    {
        public static IDisposable Bind(this GameObject text, IObservableProperty<bool> observableProperty)
        {
            return observableProperty.InvokeAndSubscribe(text.SetActive);
        }
        
        public static IDisposable Bind(this TextMeshProUGUI text, IObservableProperty<int> observableProperty, string format = "{0}")
        {
            return observableProperty.InvokeAndSubscribe(value => text.text = string.Format(format, value));
        }
        
        public static IDisposable Bind(this TextMeshProUGUI text, IObservableProperty<float> observableProperty, string format = "{0}")
        {
            return observableProperty.InvokeAndSubscribe(value => text.text = string.Format(format, value));
        }
        
        public static IDisposable Bind(this TextMeshProUGUI text, IObservableProperty<Level> observableProperty, string format = "{0}")
        {
            return observableProperty
                .InvokeAndSubscribe(level => text.text = string.Format(format, level.BoardSettings.name));
        }
        
        public static void AddTo(this IDisposable disposable, CompositeDisposable disposables)
        {
            disposables.Add(disposable);
        }
    }
}