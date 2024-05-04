using System;
using Common.Common.Code;
using UnityEngine;

namespace P2.Objectives
{
    public class MoveLimitObjective : Objective
    {
        private int _movesLeft; 
        
        private bool _isCompleted;
        public override event Action OnComplete;

        private readonly Match3 _match3;

        public MoveLimitObjective(int moveLimit, Match3 match3)
        {
            _match3 = match3;
            _movesLeft = moveLimit;
            _match3.OnMove += OnMove;
        }

        private void OnMove()
        {
            _movesLeft--;
            Debug.Log("Move, " + _movesLeft + " left");
            if (!_isCompleted && _movesLeft <= 0)
            {
                _isCompleted = true;
                OnComplete?.Invoke();
            }
        }

        public override void Dispose()
        {
            _match3.OnMove -= OnMove;
        }
    }
}