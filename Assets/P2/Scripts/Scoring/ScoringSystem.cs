using System;
using Common.Common.Code;
using P2.Observable;

namespace P2.Scoring
{
    public class ScoringSystem : IDisposable
    {
        private readonly Match3 _match3;
        
        private int _movesLeft;

        public IObservableProperty<int> Score => _score;
        private readonly ObservableProperty<int> _score = new();

        public ScoringSystem(Match3 match3)
        {
            _match3 = match3;
            
            _match3.OnGameStarted += OnGameStarted;
            _match3.OnGameEnded += OnGameEnded;
            _match3.OnMatch += OnMatch;
            _match3.OnMove += OnMove;
        }

        private void OnMove()
        {
            _movesLeft--;
        }

        private void OnMatch(int matchCount)
        {
            _score.Value += matchCount * 10;
        }

        private void OnGameStarted()
        {
            _score.Value = 0;
            _movesLeft = _match3.BoardSettings.movesLimit;
        }

        private void OnGameEnded(bool success)
        {
            if (success)
            {
                _score.Value += _movesLeft * 100;
            }
        }

        public void Dispose()
        {
            _match3.OnGameStarted -= OnGameStarted;
            _match3.OnGameEnded -= OnGameEnded;
            _match3.OnMatch -= OnMatch;
            _match3.OnMove -= OnMove;
        }
    }
}