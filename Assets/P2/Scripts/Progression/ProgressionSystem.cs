using System;
using Common.Common.Code;
using P2.Observable;
using UnityEngine;

namespace P2.Progression
{
    public class ProgressionSystem : IDisposable
    {
        public IObservableProperty<int> Level => _level;
        private readonly ObservableProperty<int> _level;
        
        private readonly Match3 _match3;

        public ProgressionSystem(Match3 match3)
        {
            _match3 = match3;
            
            var currentLevel = PlayerPrefs.GetInt("CurrentLevel", 0);
            _level = new ObservableProperty<int>(currentLevel);
            
            _match3.OnGameEnded += OnGameEnded;
        }

        private void OnGameEnded(bool win)
        {
            if (!win)
            {
                return;
            }
            
            _level.Value++;
            PlayerPrefs.SetInt("CurrentLevel", _level.Value);
        }

        public void Dispose()
        {
            _match3.OnGameEnded -= OnGameEnded;
        }
    }
}