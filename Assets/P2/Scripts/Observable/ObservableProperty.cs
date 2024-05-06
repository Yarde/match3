using System;

namespace P2.Observable
{
    public class ObservableProperty<T> : IObservableProperty<T>
    {
        private T _currentValue;

        public ObservableProperty(T initialValue = default)
        {
            _currentValue = initialValue;
        }

        public DisposableSubscription<T> OnValueChanged { get; set; }

        public T Value
        {
            get => _currentValue;
            set
            {
                if (_currentValue == null || !_currentValue.Equals(value))
                {
                    _currentValue = value;
                    OnValueChanged?.Invoke(Value);
                }
            }
        }

        public IDisposable InvokeAndSubscribe(Action<T> action)
        {
            action.Invoke(Value);
            return OnValueChanged.Subscribe(action);
        }
    }
}