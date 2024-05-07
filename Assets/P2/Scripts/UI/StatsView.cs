using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace P2.UI
{
    public class StatsView : View
    {
        [field:SerializeField] public TextMeshProUGUI MatchCount { get; private set; }
        [field:SerializeField] public TextMeshProUGUI MovesCount { get; private set; }
        [field:SerializeField] public TextMeshProUGUI TimeCount { get; private set; }
        [field:SerializeField] public TextMeshProUGUI TotalScore { get; private set; }
        
        [field:SerializeField] public TextMeshProUGUI WinCount { get; private set; }
        [field:SerializeField] public TextMeshProUGUI LoseCount { get; private set; }
        [field:SerializeField] public TextMeshProUGUI WinRate { get; private set; }
       
        [field:SerializeField] public Button CloseButton { get; private set; }

        public override void Dispose()
        {
            Destroy(gameObject);
        }
    }
}