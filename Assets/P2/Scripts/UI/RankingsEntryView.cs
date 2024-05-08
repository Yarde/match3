using P2.Rankings;
using TMPro;
using UnityEngine;

namespace P2.UI
{
    public class RankingEntryView : View
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

        public override void Dispose()
        {
        }
    }
}