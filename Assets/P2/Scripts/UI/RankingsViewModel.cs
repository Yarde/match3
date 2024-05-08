using Cysharp.Threading.Tasks;
using P2.Rankings;
using VContainer;
using VContainer.Unity;

namespace P2.UI
{
    public class RankingsViewModel : ViewModel<RankingsView>
    {
        [Inject] private RankingsSystem _rankingsSystem;
        [Inject] private WindowSystem _windowSystem;
        [Inject] private IObjectResolver _container;

        protected override void SetupInternal()
        {
            view.CloseButton.onClick.AddListener(() => _windowSystem.Pop());
            PopulateRanking().Forget();
        }

        private async UniTask PopulateRanking()
        {
            var entries = await _rankingsSystem.GetRanking();
            foreach (var entry in entries)
            {
                var rankingEntryView = _container.Instantiate(view.RankingEntryPrefab, view.RankingsParent);
                rankingEntryView.Setup(entry);
            }
        }
    }
}