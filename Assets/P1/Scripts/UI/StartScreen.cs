using System;
using System.Collections.Generic;
using Common.Code.Model;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace P1.UI
{
    public class StartScreen : Window
    {
        [SerializeField] private TextMeshProUGUI _currentLevelName;
        [SerializeField] private Button _playButton;
        [SerializeField] private Button _statsButton;
        [SerializeField] private Button _achievementsButton;
        [SerializeField] private Button _rankingButton;
        [SerializeField] private Transform _levelSelectionParent;
        [SerializeField] private LevelSelectionElement _levelSelectionPrefab;
        
        [SerializeField] private WindowManager _windowManager;

        private List<BoardSettings> _boardSettings;
        private readonly List<LevelSelectionElement> _levelElements = new();
        
        public void Setup(List<BoardSettings> boardSettings, Func<BoardSettings, UniTask> startGame)
        {
            _boardSettings = boardSettings;
            var currentLevel = PlayerPrefs.GetInt("CurrentLevel_P1", 0);
            var playerLevel = PlayerPrefs.GetInt("PlayerLevel_P1", 0);

            var selectedLevel = boardSettings[currentLevel];
            
            _currentLevelName.text = $"Level: {currentLevel}";
            _playButton.onClick.AddListener(() =>
            {
                startGame(boardSettings[PlayerPrefs.GetInt("CurrentLevel_P1", 0)]);
            });
            
            _statsButton.onClick.AddListener(() =>
            {
                _windowManager.OpenWindow<StatsWindow>();
            });
            
            _achievementsButton.onClick.AddListener(() =>
            {
                _windowManager.OpenWindow<AchievementsWindow>();
            });
            
            _rankingButton.onClick.AddListener(() =>
            {
                _windowManager.OpenWindow<RankingWindow>();
            });

            for (var i = 0; i < boardSettings.Count; i++)
            {
                var boardSetting = boardSettings[i];
                var levelSelectionElement = Instantiate(_levelSelectionPrefab, _levelSelectionParent);
                levelSelectionElement.Setup(boardSetting, i <= playerLevel, SelectLevel);
                levelSelectionElement.SetSelected(selectedLevel);
                _levelElements.Add(levelSelectionElement);
            }
        }

        private void SelectLevel(BoardSettings boardSettings)
        {
            var selectedLevel = _boardSettings.IndexOf(boardSettings);
            PlayerPrefs.SetInt("CurrentLevel_P1", selectedLevel);
            _currentLevelName.text = $"Level: {selectedLevel}";

            foreach (var level in _levelElements)
            {
                level.SetSelected(boardSettings);
            }
        }

        public void OnLevelUnlocked(int currentLevel)
        {
            for (var i = 0; i < _levelElements.Count; i++)
            {
                var level = _levelElements[i];
                level.OnLevelUnlocked(i <= currentLevel);
            }
        }
    }
}