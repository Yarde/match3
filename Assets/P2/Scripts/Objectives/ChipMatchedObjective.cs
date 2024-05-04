using System;
using Common.Common.Code;
using UnityEngine;
using VContainer;

namespace P2.Objectives
{
    public class ChipMatchedObjective : Objective
    {
        private int _matchesLeft; 
        private bool _isCompleted;
        
        public override event Action OnComplete;

        private readonly Match3 _match3;
        
        public ChipMatchedObjective(int matchesNeeded, Match3 match3)
        {
            _match3 = match3;
            _matchesLeft = matchesNeeded;
            _match3.OnMatch += OnMatch;
        }

        private void OnMatch(int matchCount)
        {
            _matchesLeft -= matchCount;
            Debug.Log("Matched " + matchCount + " chips, " + _matchesLeft + " left");
            if (!_isCompleted && _matchesLeft <= 0)
            {
                _isCompleted = true;
                OnComplete?.Invoke();
            }
        }

        public override void Dispose()
        {
            _match3.OnMatch -= OnMatch;
        }
    }
}