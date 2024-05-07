using System;
using Common.Common.Code;
using P2.Observable;
using UnityEngine;

namespace P2.Objectives
{
    public class MoveLimitObjective : Objective
    {
        public override IObservableProperty<int> Value => _movesLeft;
        private readonly ObservableProperty<int> _movesLeft; 
        
        private bool _isCompleted;
        public override DisposableSubscription<int> OnComplete { get; }

        private readonly Match3 _match3;

        public MoveLimitObjective(int moveLimit, Match3 match3)
        {
            _match3 = match3;
            _movesLeft = new ObservableProperty<int>(moveLimit);
            OnComplete = new DisposableSubscription<int>(_movesLeft);
            _match3.OnMove += OnMove;
        }

        private void OnMove()
        {
            _movesLeft.Value--;
            if (!_isCompleted && _movesLeft.Value <= 0)
            {
                _isCompleted = true;
                OnComplete?.Invoke(_movesLeft.Value);
            }
        }

        public override void Dispose()
        {
            _match3.OnMove -= OnMove;
        }
    }
}