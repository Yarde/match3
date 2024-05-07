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
            Stats.timePlayed += Time.deltaTime;
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
                Stats.wins++;
            }
            else
            {
                Stats.losses++;
            }
            var games = Stats.wins + Stats.losses;
            Stats.winLossRatio = Stats.wins / (float) games;
        }
    }
}