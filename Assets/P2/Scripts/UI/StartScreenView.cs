using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace P2.UI
{
    public class StartScreenView : View
    {
        [field:SerializeField] public TextMeshProUGUI CurrentLevelName { get; private set; }
        [field:SerializeField] public Button PlayButton { get; private set; }
        [field:SerializeField] public Button StatsButton { get; private set; }
        [field:SerializeField] public Button AchievementsButton { get; private set; }
        [field:SerializeField] public Transform LevelSelectionParent { get; private set; }
        [field:SerializeField] public LevelSelectionElement LevelSelectionPrefab { get; private set; }
        
        public override void Dispose()
        {
            Destroy(gameObject);
        }
    }
}