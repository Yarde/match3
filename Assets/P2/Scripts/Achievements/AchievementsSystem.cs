using System;
using System.Collections.Generic;
using P2.Observable;
using P2.Stats;
using P2.UI;

namespace P2.Achievements
{
    public class AchievementsSystem : IDisposable
    {
        private StatsSystem _statsSystem;
        public IReadOnlyList<Achievement> Achievements => _achievements;
        private readonly List<Achievement> _achievements;
        
        private readonly CompositeDisposable _disposables = new();
        public AchievementsSystem(StatsSystem statsSystem)
        {
            _statsSystem = statsSystem;
            
            _achievements = new List<Achievement>
            {
                new Achievement<int>("First Win", "Win a game for the first time", 1, _statsSystem.Wins),
                new Achievement<int>("10th Win", "Win a game 10 times", 10, _statsSystem.Wins),
                new Achievement<int>("100 Matches", "Do 100 matches", 100, _statsSystem.Matches),
                new Achievement<int>("1000 Matches", "Do 1000 matches", 1000, _statsSystem.Matches),
                new Achievement<int>("10000 Points", "Score 10000 points in total", 10000, _statsSystem.Score),
                new Achievement<float>("5 minutes", "Play for 5 minutes in total", 300, _statsSystem.TimePlayed)
            };
            _achievements.ForEach(a => a.AddTo(_disposables));
        }

        public void Dispose()
        {
            _disposables?.Dispose();
        }
    }
    
    public class Achievement<T> : Achievement where T : IComparable<T>
    {
        private readonly T _targetValue;
        private readonly IObservableProperty<T> _currentValue;
        
        public Achievement(string name, string description, T targetValue, IObservableProperty<T> currentValue) 
            : base(name, description)
        {
            _targetValue = targetValue;
            _currentValue = currentValue;
            
            isAchieved = new ObservableProperty<bool>();
            _currentValue.InvokeAndSubscribe(CheckAchievement).AddTo(disposables);
        }

        private void CheckAchievement(T current)
        {
            isAchieved.Value = current.CompareTo(_targetValue) >= 0;
        }
    }
    
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
}