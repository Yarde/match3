using System;
using Common.Common.Code;
using UnityEngine;

namespace P2.Scoring
{
    public class ScoringSystem : IDisposable
    {
        private readonly Match3 _match3;

        public int Score { get; private set; }

        public ScoringSystem(Match3 match3)
        {
            _match3 = match3;
            
            _match3.OnGameStarted += OnGameStarted;
            _match3.OnGameEnded += OnGameEnded;
            _match3.OnMatch += OnMatch;
        }

        private void OnMatch(int matchCount)
        {
            Score += matchCount * 10;
            Debug.Log("Score: " + Score);
        }

        private void OnGameStarted()
        {
            Score = 0;
        }

        private void OnGameEnded(bool success)
        {
            if (success)
            {
                Score += 100;
            }
            Debug.Log("Game ended with " + (success ? "success" : "failure") + ", score: " + Score);
            Unsubscribe();
        }

        public void Dispose()
        {
            Unsubscribe();
        }

        private void Unsubscribe()
        {
            _match3.OnGameStarted -= OnGameStarted;
            _match3.OnGameEnded -= OnGameEnded;
        }
    }
}