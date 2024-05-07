using System;
using UnityEngine;

namespace P1
{
    public class StatsManager : MonoBehaviour
    {
        public Stats Stats { get; private set; }
        
        private void Awake()
        {
            Stats = LoadStats();
        }

        private void Update()
        {
            Stats.TimePlayed += Time.deltaTime;
        }

        private void OnDestroy()
        {
            SaveStats(Stats);
        }

        private static Stats LoadStats()
        {
            var json = PlayerPrefs.GetString("Stats_P1", "{}");
            var stats = JsonUtility.FromJson<Stats>(json);
            return stats;
        }
        
        private static void SaveStats(Stats stats)
        {
            var json = JsonUtility.ToJson(stats);
            PlayerPrefs.SetString("Stats_P1", json);
        }

        public void OnGameFinished(bool win)
        {
            if (win)
            {
                Stats.Wins++;
            }
            else
            {
                Stats.Losses++;
            }
            var games = Stats.Wins + Stats.Losses;
            Stats.WinLossRatio = Stats.Wins / (float) games;
        }
    }

    public class Stats
    {
        public int Matches;
        public int Moves;
        
        public int Wins;
        public int Losses;
        public float WinLossRatio;
        
        public int Score;
        public float TimePlayed;
    }
}