using System;
using System.Collections.Generic;
using UnityEngine;

namespace P1
{
    public class AchievementsManager : MonoBehaviour
    {
        [SerializeField] private StatsManager _statsManager;
        
        public List<Achievement> Achievements { get; private set; }

        private void Start()
        {
            Achievements = new List<Achievement>
            {
                new()
                {
                    name = "First Win",
                    description = "Win a game for the first time",
                    isAchieved = () => _statsManager.Stats.wins >= 1
                },
                new()
                {
                    name = "10th Win",
                    description = "Win a game 10 times",
                    isAchieved = () => _statsManager.Stats.wins >= 10
                },
                new()
                {
                    name = "100 Matches",
                    description = "Do 100 matches",
                    isAchieved = () => _statsManager.Stats.matches >= 100
                },
                new()
                {
                    name = "1000 Matches",
                    description = "Do 1000 matches",
                    isAchieved = () => _statsManager.Stats.matches >= 1000
                },
                new()
                {
                    name = "10000 Points",
                    description = "Score 10000 points in total",
                    isAchieved = () => _statsManager.Stats.score >= 10000
                },
                new()
                {
                    name = "5 minutes",
                    description = "Play for 5 minutes in total",
                    isAchieved = () => _statsManager.Stats.timePlayed >= 100
                }
            };
        }
    }

    public class Achievement
    {
        public string name;
        public string description;
        public Func<bool> isAchieved;
    }
}