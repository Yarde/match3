using P2.Rankings;
using P2.Windows;
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
            _position.text = entry.Position.ToString();
            _playerName.text = entry.PlayerName;
            _score.text = entry.Score.ToString();
        }

        public override void Dispose()
        {
        }
    }
}