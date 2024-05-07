using System;
using P2.Levels;
using P2.Observable;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace P2.UI
{
    public class LevelSelectionElement : MonoBehaviour, IDisposable
    {
        [SerializeField] private TextMeshProUGUI _levelName;
        [SerializeField] private Button _button;
        [SerializeField] private Image _selectedFrame;
     
        [Inject] private LevelProgressionSystem _levelProgressionSystem;

        private Level _level;
        private readonly CompositeDisposable _disposables = new();
        
        public void Setup(Level level)
        {
            _level = level;
            _levelName.text = $"Level: {level.LevelNumber}";
            _button.onClick.AddListener(() => _levelProgressionSystem.SelectLevel(level));
            
            _levelProgressionSystem.CurrentLevel.InvokeAndSubscribe(currentLevel =>
            {
                _selectedFrame.enabled = currentLevel == level;
            }).AddTo(_disposables);
            
            _levelProgressionSystem.PlayerLevel.InvokeAndSubscribe(playerLevel =>
            {
                _button.interactable = _level.LevelNumber <= playerLevel;
            }).AddTo(_disposables);
        }

        public void Dispose()
        {
            _disposables?.Dispose();
        }
    }
}