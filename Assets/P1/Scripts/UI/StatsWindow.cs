using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace P1.UI
{
    public class StatsWindow : Window
    {
        [SerializeField] private TextMeshProUGUI _matchCount;
        [SerializeField] private TextMeshProUGUI _movesCount;
        [SerializeField] private TextMeshProUGUI _timeCount;
        [SerializeField] private TextMeshProUGUI _totalScore;
        
        [SerializeField] private TextMeshProUGUI _winCount;
        [SerializeField] private TextMeshProUGUI _loseCount;
        [SerializeField] private TextMeshProUGUI _winRate;
        
        [SerializeField] private Button _closeButton;
        
        [SerializeField] private StatsManager _statsManager;
        [SerializeField] private WindowManager _windowManager;
        
        public void Update()
        {
            var stats = _statsManager.Stats;
            
            _closeButton.onClick.AddListener(Close);
            
            _matchCount.text = $"Match: {stats.matches}";
            _movesCount.text = $"Moves: {stats.moves}";
            _timeCount.text = $"Time: {stats.timePlayed}";
            _totalScore.text = $"Total Score: {stats.score}";
            
            _winCount.text = $"Win: {stats.wins}";
            _loseCount.text = $"Lose: {stats.losses}";
            _winRate.text = $"Win Rate: {stats.winLossRatio * 100}%";
        }

        private void Close()
        {
            _windowManager.OpenWindow<StartScreen>();
        }
    }
}