using System;
using Common.Common.Code;
using UnityEngine;

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
        }
        
        public void SetObjective(int matchesNeeded, int moveLimit)
        {
            _winCondition = _factory.CreateChipMatchedObjective(matchesNeeded);
            _loseCondition = _factory.CreateMoveLimitObjective(moveLimit);
            
            _winCondition.OnComplete += OnWin;
            _loseCondition.OnComplete += OnLose;
        }

        private void OnWin()
        {
            Debug.Log("Win");
            _match3.EndGame();
        }

        private void OnLose()
        {
            Debug.Log("Lose");
            _match3.EndGame();
        }

        public void Dispose()
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
    }
}