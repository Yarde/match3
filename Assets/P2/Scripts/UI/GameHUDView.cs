using P2.Windows;
using TMPro;
using UnityEngine;

namespace P2.UI
{
    public class GameHUDView : View
    {
        [field: SerializeField] public TextMeshProUGUI ScoreText { get; private set; }
        [field: SerializeField] public TextMeshProUGUI MovesLeftText { get; private set; }
        [field: SerializeField] public TextMeshProUGUI MatchesLeftText { get; private set; }

        public override void Dispose()
        {
            Destroy(gameObject);
        }
    }
}