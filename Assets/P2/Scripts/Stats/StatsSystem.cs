using System;
using Cysharp.Threading.Tasks;
using P2.Objectives;
using P2.Observable;
using P2.Scoring;
using P2.UI;
using P2.Windows;
using UnityEngine;

namespace P2.Stats
{
    public class StatsSystem : IDisposable
    {
        private class Stats
        {
            public int matches;
            public int moves;

            public int wins;
            public int losses;
            public float winLossRatio;

            public int score;
            public float timePlayed;
        }

        public IObservableProperty<int> Matches => _matches;
        public IObservableProperty<int> Moves => _moves;
        public IObservableProperty<int> Wins => _wins;
        public IObservableProperty<int> Losses => _losses;
        public IObservableProperty<float> WinLossRatio => _winLossRatio;
        public IObservableProperty<int> Score => _score;
        public IObservableProperty<float> TimePlayed => _timePlayed;

        private readonly ObservableProperty<int> _matches;
        private readonly ObservableProperty<int> _moves;
        private readonly ObservableProperty<int> _wins;
        private readonly ObservableProperty<int> _losses;
        private readonly ObservableProperty<float> _winLossRatio;
        private readonly ObservableProperty<int> _score;
        private readonly ObservableProperty<float> _timePlayed;
        private readonly CompositeDisposable _disposable = new();
        private CompositeDisposable _winDisposable;
        private CompositeDisposable _lossDisposable;

        public StatsSystem(ScoringSystem scoringSystem, ObjectivesSystem objectivesSystem)
        {
            var stats = LoadStats();

            _matches = new ObservableProperty<int>(stats.matches);
            _moves = new ObservableProperty<int>(stats.moves);
            _wins = new ObservableProperty<int>(stats.wins);
            _losses = new ObservableProperty<int>(stats.losses);
            _winLossRatio = new ObservableProperty<float>(stats.winLossRatio);
            _score = new ObservableProperty<int>(stats.score);
            _timePlayed = new ObservableProperty<float>(stats.timePlayed);

            scoringSystem.Score
                .Subscribe(_ => _score.Value += scoringSystem.Score.Value - scoringSystem.Score.PreviousValue)
                .AddTo(_disposable);
            _wins.Subscribe(_ => _winLossRatio.Value = _wins.Value / (float)(_wins.Value + _losses.Value))
                .AddTo(_disposable);
            _losses.Subscribe(_ => _winLossRatio.Value = _wins.Value / (float)(_wins.Value + _losses.Value))
                .AddTo(_disposable);

            objectivesSystem.WinCondition.InvokeAndSubscribe(OnWinConditionChanged).AddTo(_disposable);
            objectivesSystem.LoseCondition.InvokeAndSubscribe(OnLossConditionChanged).AddTo(_disposable);

            UpdateTime().Forget();
        }

        private async UniTaskVoid UpdateTime()
        {
            while (Application.isPlaying)
            {
                _timePlayed.Value += Time.deltaTime;
                await UniTask.Yield(PlayerLoopTiming.Update);
            }
        }

        private void OnWinConditionChanged(Objective obj)
        {
            _winDisposable?.Dispose();
            _winDisposable = new CompositeDisposable();
            if (obj == null) return;

            obj.Value.Subscribe(_ => _matches.Value += obj.Value.PreviousValue - obj.Value.Value).AddTo(_winDisposable);
            obj.OnComplete.Subscribe(_ => _wins.Value++).AddTo(_winDisposable);
        }

        private void OnLossConditionChanged(Objective obj)
        {
            _lossDisposable?.Dispose();
            _lossDisposable = new CompositeDisposable();
            if (obj == null) return;

            obj.Value.Subscribe(_ => _moves.Value += obj.Value.PreviousValue - obj.Value.Value).AddTo(_lossDisposable);
            obj.OnComplete.Subscribe(_ => _losses.Value++).AddTo(_lossDisposable);
        }

        private static Stats LoadStats()
        {
            var json = PlayerPrefs.GetString("Stats_P2", "{}");
            var stats = JsonUtility.FromJson<Stats>(json);
            return stats;
        }

        private static void SaveStats(Stats stats)
        {
            var json = JsonUtility.ToJson(stats);
            PlayerPrefs.SetString("Stats_P2", json);
        }

        public void Dispose()
        {
            SaveStats(new Stats
            {
                matches = _matches.Value,
                moves = _moves.Value,
                wins = _wins.Value,
                losses = _losses.Value,
                winLossRatio = _winLossRatio.Value,
                score = _score.Value,
                timePlayed = _timePlayed.Value
            });
            _disposable?.Dispose();
            _winDisposable?.Dispose();
            _lossDisposable?.Dispose();
        }
    }
}