using System;
using Common.Common.Code;
using Cysharp.Threading.Tasks;

namespace P2.Objectives
{
    public class ObjectivesSystem : IDisposable
    {
        private Objective _winCondition;
        private Objective _loseCondition;
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
            _winCondition = _factory.CreateChipMatchedObjective(_match3.BoardSettings.matchesNeeded);
            _loseCondition = _factory.CreateMoveLimitObjective(_match3.BoardSettings.movesLimit);

            _winCondition.OnComplete += OnWin;
            _loseCondition.OnComplete += OnLose;
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
            if (_winCondition != null)
            {
                _winCondition.OnComplete -= OnWin;
                _winCondition.Dispose();
            }
            if (_loseCondition != null)
            {
                _loseCondition.OnComplete -= OnLose;
                _loseCondition.Dispose();
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