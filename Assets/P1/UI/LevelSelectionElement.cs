using System;
using Common.Code.Model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace P1.UI
{
    public class LevelSelectionElement : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _levelName;
        [SerializeField] private Button _button;
        [SerializeField] private Image _selectedFrame;
        private BoardSettings _boardSetting;

        public void Setup(BoardSettings boardSetting, bool isUnlocked, Action<BoardSettings> selectLevel)
        {
            _boardSetting = boardSetting;
            
            _levelName.text = $"Level: {boardSetting.name}";
            _button.interactable = isUnlocked;
            _button.onClick.AddListener(() => selectLevel(boardSetting));
            
            _selectedFrame.enabled = false;
        }
        
        public void SetSelected(BoardSettings selectedLevel)
        {
            _selectedFrame.enabled = selectedLevel == _boardSetting;
        }
        
        public void OnLevelUnlocked(bool isUnlocked)
        {
            _button.interactable = isUnlocked;
        }
    }
}