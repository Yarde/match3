using System;
using Common.Code.Model;
using P2.Observable;
using P2.UI;

namespace P2.Levels
{
    public class Level : IDisposable
    {
        public BoardSettings BoardSettings { get; private set; }
        public int LevelNumber { get; private set; }
        public bool IsUnlocked { get; private set; }

        private readonly CompositeDisposable _disposables = new();

        public Level(int levelNumber, BoardSettings boardSettings, IObservableProperty<int> currentLevel)
        {
            LevelNumber = levelNumber;
            BoardSettings = boardSettings;

            currentLevel.InvokeAndSubscribe(OnLevelUp).AddTo(_disposables);
        }

        private void OnLevelUp(int level)
        {
            IsUnlocked = level >= LevelNumber;
        }

        public void Dispose()
        {
            _disposables?.Dispose();
        }
    }
}