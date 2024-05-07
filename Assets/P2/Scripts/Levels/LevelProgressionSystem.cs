using System;
using System.Collections.Generic;
using System.Linq;
using Common.Common.Code;
using P2.Observable;
using P2.UI;
using UnityEngine;

namespace P2.Levels
{
    public class LevelProgressionSystem : IDisposable
    {
        public IObservableProperty<int> PlayerLevel => _playerLevel;
        public IReadOnlyList<Level> AllLevels => _allLevels;
        public IReadOnlyList<Level> UnlockedLevels => _unlockedLevels;
        public Level CurrentLevel => _unlockedLevels[Mathf.Min(_playerLevel.Value, _unlockedLevels.Count - 1)];
        
        private List<Level> _unlockedLevels;
        private readonly ObservableProperty<int> _playerLevel;
        
        private readonly Match3 _match3;
        private readonly List<Level> _allLevels;
        private readonly CompositeDisposable _disposables = new();
        
        public LevelProgressionSystem(Match3 match3)
        {
            _match3 = match3;
            
            var currentLevel = PlayerPrefs.GetInt("CurrentLevel", 0);
            _playerLevel = new ObservableProperty<int>(currentLevel);

            var levelList = Resources.Load<LevelList>("LevelList");
            var levels = levelList._levels;
            
            _allLevels = new List<Level>();
            for (var i = 0; i < levels.Count; i++)
            {
                _allLevels.Add(new Level(i, levels[i], _playerLevel));
            }

            _unlockedLevels = _allLevels.GetRange(0, _playerLevel.Value + 1);
            
            _match3.OnGameEnded += OnGameEnded;
            _playerLevel.InvokeAndSubscribe(OnLevelUnlocked).AddTo(_disposables);
        }

        private void OnLevelUnlocked(int newMaxLevel)
        {
            var count = Mathf.Min(newMaxLevel + 1, _allLevels.Count);
            _unlockedLevels = _allLevels.GetRange(0,  count);
        }

        private void OnGameEnded(bool win)
        {
            if (!win)
            {
                return;
            }

            var playedLevel = _match3.BoardSettings;
            var level = _allLevels.First(level => level.BoardSettings == playedLevel);
            if (level.IsUnlocked)
            {
                return;
            }
            
            _playerLevel.Value++;
            PlayerPrefs.SetInt("CurrentLevel", _playerLevel.Value);
        }

        public void Dispose()
        {
            _match3.OnGameEnded -= OnGameEnded;
            _disposables?.Dispose();
        }
    }
}