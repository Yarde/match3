using P2.Windows;
using UnityEngine;
using UnityEngine.UI;

namespace P2.UI
{
    public class AchievementsView : View
    {
        [field: SerializeField] public Transform AchievementsParent { get; private set; }
        [field: SerializeField] public AchievementView AchievementPrefab { get; private set; }

        [field: SerializeField] public Button CloseButton { get; private set; }

        public override void Dispose()
        {
            Destroy(gameObject);
        }
    }
}