using Common.Common.Code;
using P2.Observable;

namespace P2.Objectives
{
    public class ChipMatchedObjective : Objective
    {
        public override IObservableProperty<int> Value => _matchesLeft;
        private readonly ObservableProperty<int> _matchesLeft;
        private bool _isCompleted;

        public override DisposableSubscription<int> OnComplete { get; }

        private readonly Match3 _match3;

        public ChipMatchedObjective(int matchesNeeded, Match3 match3)
        {
            _match3 = match3;
            _matchesLeft = new ObservableProperty<int>(matchesNeeded);
            OnComplete = new DisposableSubscription<int>(_matchesLeft);
            _match3.OnMatch += OnMatch;
        }

        private void OnMatch(int matchCount)
        {
            _matchesLeft.Value -= matchCount;
            if (!_isCompleted && _matchesLeft.Value <= 0)
            {
                _isCompleted = true;
                OnComplete?.Invoke(_matchesLeft.Value);
            }
        }

        public override void Dispose()
        {
            _match3.OnMatch -= OnMatch;
        }
    }
}