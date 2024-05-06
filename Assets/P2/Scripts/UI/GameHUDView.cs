using System;
using P2.Objectives;
using P2.Observable;
using P2.Scoring;
using TMPro;
using UnityEngine;
using VContainer;
using Object = UnityEngine.Object;

namespace P2.UI
{
    public class GameHUDView : View
    {
        [field:SerializeField] public TextMeshProUGUI ScoreText { get; private set; }
        [field:SerializeField] public TextMeshProUGUI MovesLeftText { get; private set; }
        [field:SerializeField] public TextMeshProUGUI MatchesLeftText { get; private set; }
        
        public override void Dispose()
        {
            Destroy(gameObject);
        }
    }
    
    public class GameHUDViewModel : ViewModel<GameHUDView>
    {
        [Inject] private ScoringSystem _scoringSystem;
        [Inject] private ObjectivesSystem _objectivesSystem;

        protected override void ShowInternal()
        {
            view.ScoreText.Bind(_scoringSystem.Score, "Score: {0}").AddTo(disposables);
            view.MovesLeftText.Bind(_objectivesSystem.LoseCondition.Value, "Moves left: {0}").AddTo(disposables);
            view.MatchesLeftText.Bind(_objectivesSystem.WinCondition.Value, "Matches left: {0}").AddTo(disposables);
        }
    }

    public abstract class ViewModel<T> : ViewModel where T : View
    {
        protected T view;
        protected readonly CompositeDisposable disposables = new();
        
        public sealed override void Show()
        {
            var prefab = Resources.Load<T>(typeof(T).Name);
            view = Object.Instantiate(prefab);
            ShowInternal();
        }
        
        public void Close()
        {
            disposables.Dispose();
            view.Dispose();
        }

        protected abstract void ShowInternal();
    }

    public abstract class View : MonoBehaviour, IDisposable
    {
        public abstract void Dispose();
    }

    public abstract class ViewModel
    {
        public abstract void Show();
    }

    public static class BindingExtensions
    {
        public static IDisposable Bind(this TextMeshProUGUI text, IObservableProperty<int> observableProperty, string format = "{0}")
        {
            return observableProperty.InvokeAndSubscribe(value => text.text = string.Format(format, value));
        }
        
        public static void AddTo(this IDisposable disposable, CompositeDisposable disposables)
        {
            disposables.Add(disposable);
        }
    }
}