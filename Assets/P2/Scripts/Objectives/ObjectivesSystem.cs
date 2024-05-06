using System;
using Common.Common.Code;
using Cysharp.Threading.Tasks;

namespace P2.Objectives
{
    public class ObjectivesSystem : IDisposable
    {
        public Objective WinCondition { get; private set; }
        public Objective LoseCondition { get; private set; }
        private readonly Match3 _match3;
        private readonly ObjectivesFactory _factory;

        public ObjectivesSystem(Match3 match3, ObjectivesFactory factory)
        {
            _match3 = match3;
            _factory = factory;
            
            _match3.OnGameStarted += OnGameStarted;
            _match3.OnGameEnded += OnGameEnded;
        }

        private void OnGameStarted()
        {
            WinCondition = _factory.CreateChipMatchedObjective(_match3.BoardSettings.matchesNeeded);
            LoseCondition = _factory.CreateMoveLimitObjective(_match3.BoardSettings.movesLimit);

            WinCondition.OnComplete += OnWin;
            LoseCondition.OnComplete += OnLose;
        }

        private void OnGameEnded(bool obj)
        {
            Unsubscribe();
        }

        private void OnWin()
        {
            _match3.EndGame(true).Forget();
        }

        private void OnLose()
        {
            _match3.EndGame(false).Forget();
        }
        
        private void Unsubscribe()
        {
            if (WinCondition != null)
            {
                WinCondition.OnComplete -= OnWin;
                WinCondition.Dispose();
            }
            if (LoseCondition != null)
            {
                LoseCondition.OnComplete -= OnLose;
                LoseCondition.Dispose();
            }
        }

        public void Dispose()
        {
            Unsubscribe();
            _match3.OnGameStarted -= OnGameStarted;
            _match3.OnGameEnded -= OnGameEnded;
        }
    }
}