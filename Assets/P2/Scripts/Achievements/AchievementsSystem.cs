using System;
using System.Collections.Generic;
using P2.Observable;
using P2.Stats;
using P2.UI;
using P2.Windows;

namespace P2.Achievements
{
    public class AchievementsSystem : IDisposable
    {
        public IReadOnlyList<Achievement> Achievements => _achievements;
        private readonly List<Achievement> _achievements;

        private readonly CompositeDisposable _disposables = new();

        public AchievementsSystem(StatsSystem statsSystem)
        {
            _achievements = new List<Achievement>
            {
                new Achievement<int>("First Win", "Win a game for the first time", 1, statsSystem.Wins),
                new Achievement<int>("10th Win", "Win a game 10 times", 10, statsSystem.Wins),
                new Achievement<int>("100 Matches", "Do 100 matches", 100, statsSystem.Matches),
                new Achievement<int>("1000 Matches", "Do 1000 matches", 1000, statsSystem.Matches),
                new Achievement<int>("10000 Points", "Score 10000 points in total", 10000, statsSystem.Score),
                new Achievement<float>("5 minutes", "Play for 5 minutes in total", 300f, statsSystem.TimePlayed)
            };
            _achievements.ForEach(a => a.AddTo(_disposables));
        }

        public void Dispose()
        {
            _disposables?.Dispose();
        }
    }
}