using TMPro;
using UnityEngine;

namespace P1.UI
{
    public class RankingElement : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _position;
        [SerializeField] private TextMeshProUGUI _playerName;
        [SerializeField] private TextMeshProUGUI _score;
        
        public void Setup(RankingEntry entry)
        {
            _position.text = entry.position.ToString();
            _playerName.text = entry.playerName;
            _score.text = entry.score.ToString();
        }
    }
}