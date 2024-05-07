using TMPro;
using UnityEngine;

namespace P1.UI
{
    public class GameHUD : Window
    {
        [SerializeField] private TextMeshProUGUI _scoreText;
        [SerializeField] private TextMeshProUGUI _movesLeftText;
        [SerializeField] private TextMeshProUGUI _matchesLeftText;
        
        public void Setup(int score, int movesLeft, int matchesLeft)
        {
            _scoreText.text = "Score: " + score;
            _movesLeftText.text = "Moves left: " + movesLeft;
            _matchesLeftText.text = "Matches left: " + matchesLeft;
        }
    }
}