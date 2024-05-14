using System;
using Common.Common.Code;
using Cysharp.Threading.Tasks;
using P2.Observable;
using P2.UI;
using P2.Windows;

namespace P2.Objectives
{
    public class ObjectivesSystem : IDisposable
    {
        public ObservableProperty<Objective> WinCondition { get; private set; }
        public ObservableProperty<Objective> LoseCondition { get; private set; }
        private readonly Match3 _match3;
        private readonly ObjectivesFactory _factory;
        private CompositeDisposable _disposables;

        public ObjectivesSystem(Match3 match3, ObjectivesFactory factory)
        {
            _match3 = match3;
            _factory = factory;

            WinCondition = new ObservableProperty<Objective>();
            LoseCondition = new ObservableProperty<Objective>();

            _match3.OnGameStarted += OnGameStarted;
            _match3.OnGameEnded += OnGameEnded;
        }

        private void OnGameStarted()
        {
            _disposables = new CompositeDisposable();

            WinCondition.Value = _factory.CreateChipMatchedObjective(_match3.BoardSettings.matchesNeeded);
            LoseCondition.Value = _factory.CreateMoveLimitObjective(_match3.BoardSettings.movesLimit);

            WinCondition.Value.AddTo(_disposables);
            LoseCondition.Value.AddTo(_disposables);
            WinCondition.Value.OnComplete.Subscribe(OnWin).AddTo(_disposables);
            LoseCondition.Value.OnComplete.Subscribe(OnLose).AddTo(_disposables);
        }

        private void OnGameEnded(bool obj)
        {
            _disposables?.Dispose();
        }

        private void OnWin(int _)
        {
            _match3.EndGame(true).Forget();
        }

        private void OnLose(int _)
        {
            _match3.EndGame(false).Forget();
        }

        public void Dispose()
        {
            _disposables?.Dispose();
            _match3.OnGameStarted -= OnGameStarted;
            _match3.OnGameEnded -= OnGameEnded;
        }
    }
}