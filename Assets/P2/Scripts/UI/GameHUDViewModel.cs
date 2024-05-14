using P2.Objectives;
using P2.Scoring;
using P2.Windows;
using VContainer;

namespace P2.UI
{
    public class GameHUDViewModel : ViewModel<GameHUDView>
    {
        [Inject] private ScoringSystem _scoringSystem;
        [Inject] private ObjectivesSystem _objectivesSystem;

        protected override void SetupInternal()
        {
            view.ScoreText.Bind(_scoringSystem.Score, "Score: {0}").AddTo(disposables);
            view.MovesLeftText.Bind(_objectivesSystem.LoseCondition.Value.Value, "Moves left: {0}").AddTo(disposables);
            view.MatchesLeftText.Bind(_objectivesSystem.WinCondition.Value.Value, "Matches left: {0}")
                .AddTo(disposables);
        }
    }
}