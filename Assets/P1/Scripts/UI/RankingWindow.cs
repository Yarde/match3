using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace P1.UI
{
    public class RankingWindow : Window
    {
        [SerializeField] private Transform _rankingParent;
        [SerializeField] private RankingElement _rankingElementPrefab;
        
        [SerializeField] private Button _closeButton;
        
        [SerializeField] private RankingManager _rankingManager;
        [SerializeField] private WindowManager _windowManager;
        
        public void Start()
        {
            var getRankingTask = _rankingManager.GetRanking();
            
            _closeButton.onClick.AddListener(Close);
            
            SetupRanking(getRankingTask).Forget();
        }

        private async UniTask SetupRanking(UniTask<IReadOnlyList<RankingEntry>> getRankingTask)
        {
            var entries = await getRankingTask;
            foreach (var entry in entries)
            {
                var element = Instantiate(_rankingElementPrefab, _rankingParent);
                element.Setup(entry);
            }
        }

        private void Close()
        {
            _windowManager.OpenWindow<StartScreen>();
        }
    }
}