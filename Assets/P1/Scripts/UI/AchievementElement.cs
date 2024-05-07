using TMPro;
using UnityEngine;

namespace P1.UI
{
    public class AchievementElement : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _name;
        [SerializeField] private TextMeshProUGUI _description;
        [SerializeField] private GameObject _achieved;
        
        private Achievement _achievement;

        public void Setup(Achievement achievement)
        {
            _achievement = achievement;
            _name.text = achievement.name;
            _description.text = achievement.description;
            _achieved.SetActive(achievement.isAchieved());
        }

        private void Update()
        {
            _achieved.SetActive(_achievement.isAchieved());
        }
    }
}