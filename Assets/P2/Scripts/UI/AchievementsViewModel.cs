using P2.Achievements;
using VContainer;
using VContainer.Unity;

namespace P2.UI
{
    public class AchievementsViewModel : ViewModel<AchievementsView>
    {
        [Inject] private AchievementsSystem _achievementsSystem;
        [Inject] private WindowSystem _windowSystem;
        [Inject] private IObjectResolver _container;

        protected override void SetupInternal()
        {
            foreach (var achievement in _achievementsSystem.Achievements)
            {
                var achievementView = _container.Instantiate(view.AchievementPrefab, view.AchievementsParent);
                achievementView.Setup(achievement);
                achievementView.AddTo(disposables);
            }
            
            view.CloseButton.onClick.AddListener(() => _windowSystem.Pop());
        }
    }
}