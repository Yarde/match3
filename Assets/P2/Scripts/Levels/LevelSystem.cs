using System;
using System.Collections.Generic;
using Common.Code.Model;
using JetBrains.Annotations;
using P2.Observable;
using P2.Progression;
using P2.UI;
using UnityEngine;

namespace P2.Levels
{
    public class LevelSystem : IDisposable
    {
        public IReadOnlyList<BoardSettings> Levels => _levelList._levels;
        public IReadOnlyList<BoardSettings> UnlockedLevels => _unlockedLevels;
        public BoardSettings CurrentLevel => _unlockedLevels[Mathf.Min(_progressionSystem.Level.Value, _unlockedLevels.Count - 1)];
        
        private readonly LevelList _levelList;
        private readonly ProgressionSystem _progressionSystem;
        private List<BoardSettings> _unlockedLevels;
        private readonly CompositeDisposable _disposables = new();

        [UsedImplicitly]
        public LevelSystem(ProgressionSystem progressionSystem)
        {
            _progressionSystem = progressionSystem;
            _levelList = Resources.Load<LevelList>("LevelList");
            _unlockedLevels = _levelList._levels.GetRange(0, progressionSystem.Level.Value + 1);
            
            _progressionSystem.Level.InvokeAndSubscribe(OnLevelUnlocked).AddTo(_disposables);
        }

        private void OnLevelUnlocked(int newMaxLevel)
        {
            var levels = _levelList._levels;
            _unlockedLevels = levels.GetRange(0,  Mathf.Min(newMaxLevel + 1, levels.Count));
        }

        public void Dispose()
        {
            _disposables?.Dispose();
        }
    }
}