using System;
using Common.Code.Model;
using Common.Common.Code;
using Cysharp.Threading.Tasks;
using P2.Objectives;
using P2.Scoring;
using P2.UI;

namespace P2.Gameplay
{
    public class GameplaySystem : IDisposable
    {
        private readonly Match3 _match3;
        private readonly WindowSystem _windowSystem;
        private readonly ScoringSystem _scoringSystem;
        private readonly ObjectivesSystem _objectivesSystem;
        private GameHUDViewModel _hud;

        public GameplaySystem(Match3 match3, WindowSystem windowSystem, 
            ScoringSystem scoringSystem, ObjectivesSystem objectivesSystem)
        {
           _match3 = match3;
           _windowSystem = windowSystem;
           _scoringSystem = scoringSystem;
           _objectivesSystem = objectivesSystem;
           
           _match3.OnGameEnded += OnGameEnded;
        }

        public async UniTask StartGame(BoardSettings boardSettings)
        {
            await _match3.StartGame(boardSettings);
            _hud = _windowSystem.OpenWindow<GameHUDViewModel>();
        }

        private void OnGameEnded(bool successful)
        {
            _hud.Close();
        }

        public void Dispose()
        {
            _match3.OnGameEnded -= OnGameEnded;
        }
    }
}