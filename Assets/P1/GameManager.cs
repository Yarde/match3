using System.Collections.Generic;
using Common.Code.Model;
using Common.Common.Code;
using Cysharp.Threading.Tasks;
using P1.UI;
using UnityEngine;

namespace P1
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private List<BoardSettings> _boardSettings;
        [SerializeField] private WindowManager _windowManager;

        private Match3 _match3;
        private GameHUD _hud;
        private StartScreen _startScreen;
        private int _movesLeft;
        private int _matchesLeft;
        private int _score;
        private bool _isGameEnded;

        private void Start()
        {
            _match3 = new Match3();
            
            _match3.OnGameStarted += OnGameStarted;
            _match3.OnGameEnded += OnGameEnded;
            _match3.OnMatch += OnMatch;
            _match3.OnMove += OnMove;

            _startScreen = _windowManager.OpenWindow<StartScreen>();
            _startScreen.Setup(_boardSettings, StartGame);
        }
        
        private async UniTask StartGame(BoardSettings settings)
        {
            _movesLeft = settings.movesLimit;
            _matchesLeft = settings.matchesNeeded;
            
            _hud = _windowManager.OpenWindow<GameHUD>();
            await _match3.StartGame(settings);
        }

        private void OnGameStarted()
        {
            _isGameEnded = false;
            _score = 0;
            _hud.Setup(_score, _movesLeft, _matchesLeft);
        }

        private void OnGameEnded(bool success)
        {
            _score += success ? _movesLeft * 100 : 0;
            _hud.Setup(_score, _movesLeft, _matchesLeft);

            var currentLevel = PlayerPrefs.GetInt("CurrentLevel_P1", 0);
            var playerLevel = PlayerPrefs.GetInt("PlayerLevel_P1", 0);
            if (success && currentLevel >= playerLevel && currentLevel < _boardSettings.Count - 1)
            {
                PlayerPrefs.SetInt("PlayerLevel_P1", currentLevel + 1);
                _startScreen.OnLevelUnlocked(currentLevel + 1);
            }
            
            _windowManager.OpenWindow<StartScreen>();
        }

        private void OnDestroy()
        {
            _match3.OnGameStarted -= OnGameStarted;
            _match3.OnGameEnded -= OnGameEnded;
            _match3.OnMatch -= OnMatch;
            _match3.OnMove -= OnMove;
        }

        private void OnMove()
        {
            _movesLeft--;
            _hud.Setup(_score, _movesLeft, _matchesLeft);
            if (!_isGameEnded && _movesLeft <= 0)
            {
                _isGameEnded = true;
                _match3.EndGame(false).Forget();
            }
        }

        private void OnMatch(int matchCount)
        {
            _matchesLeft -= matchCount;
            _score += matchCount * 10;
            _hud.Setup(_score, _movesLeft, _matchesLeft);
            if (!_isGameEnded && _matchesLeft <= 0)
            {
                _isGameEnded = true;
                _match3.EndGame(true).Forget();
            }
        }
    }
}