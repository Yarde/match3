using P2.Stats;
using P2.Windows;
using VContainer;

namespace P2.UI
{
    public class StatsViewModel : ViewModel<StatsView>
    {
        [Inject] private StatsSystem _statsSystem;
        [Inject] private WindowSystem _windowSystem;

        protected override void SetupInternal()
        {
            view.MatchCount.Bind(_statsSystem.Matches, "Matches: {0}").AddTo(disposables);
            view.MovesCount.Bind(_statsSystem.Moves, "Moves: {0}").AddTo(disposables);
            view.TimeCount.Bind(_statsSystem.TimePlayed, "Time played: {0}").AddTo(disposables);
            view.TotalScore.Bind(_statsSystem.Score, "Total score: {0}").AddTo(disposables);
            view.WinCount.Bind(_statsSystem.Wins, "Wins: {0}").AddTo(disposables);
            view.LoseCount.Bind(_statsSystem.Losses, "Losses: {0}").AddTo(disposables);
            view.WinRate.Bind(_statsSystem.WinLossRatio, "Win rate: {0:P}").AddTo(disposables);

            view.CloseButton.onClick.AddListener(() => _windowSystem.Pop());
        }
    }
}