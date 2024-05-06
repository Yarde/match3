using P2.Objectives;
using P2.Observable;
using P2.Scoring;
using TMPro;
using UnityEngine;
using VContainer;

namespace P2.UI
{
    public class GameHUDView : MonoBehaviour
    {
        [field:SerializeField] public TextMeshProUGUI ScoreText { get; private set; }
        [field:SerializeField] public TextMeshProUGUI MovesLeftText { get; private set; }
        [field:SerializeField] public TextMeshProUGUI MatchesLeftText { get; private set; }
    }
    
    public class GameHUDViewModel : ViewModel<GameHUDView>
    {
        [Inject] private ScoringSystem _scoringSystem;
        [Inject] private ObjectivesSystem _objectivesSystem;
        
        public override void ShowInternal()
        {
            view.ScoreText.Bind(_scoringSystem.Score);
        }
    }

    public abstract class ViewModel<T> : ViewModel where T : MonoBehaviour
    {
        protected T view;
        
        public sealed override void Show()
        {
            var prefab = Resources.Load<T>(typeof(T).Name);
            view = Object.Instantiate(prefab);
            ShowInternal();
        }

        public abstract void ShowInternal();
    }
    
    public abstract class ViewModel
    {
        public abstract void Show();
    }

    public static class BindingExtensions
    {
        public static void Bind(this TextMeshProUGUI text, IObservableProperty<int> observableProperty)
        {
            observableProperty.InvokeAndSubscribe(value => text.text = value.ToString());
        }
    }
}