using P2.Windows;
using UnityEngine;
using UnityEngine.UI;

namespace P2.UI
{
    public class RankingsView : View
    {
        [field: SerializeField] public Transform RankingsParent { get; private set; }
        [field: SerializeField] public RankingEntryView RankingEntryPrefab { get; private set; }

        [field: SerializeField] public Button CloseButton { get; private set; }

        public override void Dispose()
        {
            Destroy(gameObject);
        }
    }
}