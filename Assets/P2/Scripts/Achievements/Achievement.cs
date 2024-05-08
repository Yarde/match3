using System;
using P2.Observable;
using P2.UI;

namespace P2.Achievements
{
    public abstract class Achievement : IDisposable
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public IObservableProperty<bool> IsAchieved => isAchieved;
        
        protected ObservableProperty<bool> isAchieved = new();
        protected readonly CompositeDisposable disposables = new();

        protected Achievement(string name, string description)
        {
            Name = name;
            Description = description;
        }
        
        public void Dispose()
        {
            disposables?.Dispose();
        }
    }

    public class Achievement<T> : Achievement where T : IComparable<T>
    {
        private readonly T _targetValue;

        public Achievement(string name, string description, T targetValue, IObservableProperty<T> currentValue)
            : base(name, description)
        {
            _targetValue = targetValue;

            isAchieved = new ObservableProperty<bool>();
            currentValue.InvokeAndSubscribe(CheckAchievement).AddTo(disposables);
        }

        private void CheckAchievement(T current)
        {
            isAchieved.Value = current.CompareTo(_targetValue) >= 0;
        }
    }
}