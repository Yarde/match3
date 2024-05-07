using System;
using UnityEngine;
using UnityEngine.UI;

namespace P1.UI
{
    public class AchievementsWindow : Window
    {
        [SerializeField] private Transform _achievementsParent;
        [SerializeField] private AchievementElement _achievementElementPrefab;
        
        [SerializeField] private Button _closeButton;
        
        [SerializeField] private AchievementsManager _achievementsManager;
        [SerializeField] private WindowManager _windowManager;
        
        public void Start()
        {
            var achievements = _achievementsManager.Achievements;
            
            _closeButton.onClick.AddListener(Close);
            
            foreach (var achievement in achievements)
            {
                var achievementElement = Instantiate(_achievementElementPrefab, _achievementsParent);
                achievementElement.Setup(achievement);
            }
        }

        private void Close()
        {
            _windowManager.OpenWindow<StartScreen>();
        }
    }
}