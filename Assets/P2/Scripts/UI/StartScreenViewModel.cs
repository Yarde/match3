using P2.Gameplay;
using P2.Levels;
using P2.Windows;
using VContainer;
using VContainer.Unity;

namespace P2.UI
{
    public class StartScreenViewModel : ViewModel<StartScreenView>
    {
        [Inject] private LevelProgressionSystem _levelProgressionSystem;
        [Inject] private GameplaySystem _gameplaySystem;
        [Inject] private WindowSystem _windowSystem;
        [Inject] private IObjectResolver _container;

        protected override void SetupInternal()
        {
            view.CurrentLevelName.Bind(_levelProgressionSystem.CurrentLevel, "Level: {0}").AddTo(disposables);
            view.PlayButton.onClick.AddListener(StartGame);
            view.StatsButton.onClick.AddListener(() => _windowSystem.Push<StatsViewModel>());
            view.AchievementsButton.onClick.AddListener(() => _windowSystem.Push<AchievementsViewModel>());
            view.RankingsButton.onClick.AddListener(() => _windowSystem.Push<RankingsViewModel>());

            foreach (var level in _levelProgressionSystem.AllLevels)
            {
                var levelSelectionElement =
                    _container.Instantiate(view.LevelSelectionPrefab, view.LevelSelectionParent);
                levelSelectionElement.Setup(level);
            }
        }

        private async void StartGame()
        {
            await _gameplaySystem.StartGame(_levelProgressionSystem.CurrentLevel.Value);
        }
    }
}