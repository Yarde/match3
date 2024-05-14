using System;
using Common.Common.Code;
using Cysharp.Threading.Tasks;
using P2.Levels;
using P2.Objectives;
using P2.Scoring;
using P2.UI;
using P2.Windows;

namespace P2.Gameplay
{
    public class GameplaySystem : IDisposable
    {
        private readonly Match3 _match3;
        private readonly WindowSystem _windowSystem;
        private GameHUDViewModel _hud;

        public GameplaySystem(Match3 match3, WindowSystem windowSystem,
            ScoringSystem scoringSystem, ObjectivesSystem objectivesSystem)
        {
            _match3 = match3;
            _windowSystem = windowSystem;

            _match3.OnGameEnded += OnGameEnded;
        }

        public async UniTask StartGame(Level level)
        {
            await _match3.StartGame(level.BoardSettings);
            _windowSystem.Push<GameHUDViewModel>();
        }

        private void OnGameEnded(bool successful)
        {
            _windowSystem.Pop();
        }

        public void Dispose()
        {
            _match3.OnGameEnded -= OnGameEnded;
        }
    }
}