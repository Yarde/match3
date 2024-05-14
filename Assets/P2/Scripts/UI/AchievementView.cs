using P2.Achievements;
using P2.Observable;
using P2.Windows;
using TMPro;
using UnityEngine;

namespace P2.UI
{
    public class AchievementView : View
    {
        [SerializeField] private TextMeshProUGUI _name;
        [SerializeField] private TextMeshProUGUI _description;
        [SerializeField] private GameObject _achieved;

        private readonly CompositeDisposable _disposables = new();

        public void Setup(Achievement achievement)
        {
            _name.text = achievement.Name;
            _description.text = achievement.Description;

            _achieved.Bind(achievement.IsAchieved).AddTo(_disposables);
        }

        public override void Dispose()
        {
            _disposables?.Dispose();
        }
    }
}