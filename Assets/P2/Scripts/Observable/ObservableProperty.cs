using System;

namespace P2.Observable
{
    public class ObservableProperty<T> : IObservableProperty<T>
    {
        private T _currentValue;

        public ObservableProperty(T initialValue = default)
        {
            OnValueChanged = new DisposableSubscription<T>(this);
            PreviousValue = _currentValue = initialValue;
        }

        public DisposableSubscription<T> OnValueChanged { get; set; }

        public T Value
        {
            get => _currentValue;
            set
            {
                if (_currentValue == null || !_currentValue.Equals(value))
                {
                    PreviousValue = _currentValue;
                    _currentValue = value;
                    OnValueChanged?.Invoke(Value);
                }
            }
        }
        
        public T PreviousValue { get; private set; }

        public IDisposable InvokeAndSubscribe(Action<T> action)
        {
            action.Invoke(Value);
            return Subscribe(action);
        }
        
        public IDisposable Subscribe(Action<T> action)
        {
            return OnValueChanged.Subscribe(action);
        }
    }
}