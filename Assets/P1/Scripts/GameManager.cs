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
        [SerializeField] private StatsManager _statsManager;
        
        public Match3 Match3 { get; private set; }
        public int MovesLeft { get; private set; }
        public int MatchesLeft { get; private set; }
        public int Score { get; private set; }
        
        private GameHUD _hud;
        private StartScreen _startScreen;
        private bool _isGameEnded;

        private void Start()
        {
            Match3 = new Match3();
            
            Match3.OnGameStarted += OnGameStarted;
            Match3.OnGameEnded += OnGameEnded;
            Match3.OnMatch += OnMatch;
            Match3.OnMove += OnMove;

            _startScreen = _windowManager.OpenWindow<StartScreen>();
            _startScreen.Setup(_boardSettings, StartGame);
        }
        
        private async UniTask StartGame(BoardSettings settings)
        {
            MovesLeft = settings.movesLimit;
            MatchesLeft = settings.matchesNeeded;
            
            _hud = _windowManager.OpenWindow<GameHUD>();
            await Match3.StartGame(settings);
        }

        private void OnGameStarted()
        {
            _isGameEnded = false;
            Score = 0;
            _hud.Setup(Score, MovesLeft, MatchesLeft);
        }

        private void OnGameEnded(bool success)
        {
            var gain = success ? MovesLeft * 100 : 0;
            Score += gain;
            _statsManager.Stats.Score += gain;
            _hud.Setup(Score, MovesLeft, MatchesLeft);

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
            Match3.OnGameStarted -= OnGameStarted;
            Match3.OnGameEnded -= OnGameEnded;
            Match3.OnMatch -= OnMatch;
            Match3.OnMove -= OnMove;
        }

        private void OnMove()
        {
            MovesLeft--;
            _statsManager.Stats.Moves++;
            
            _hud.Setup(Score, MovesLeft, MatchesLeft);
            if (!_isGameEnded && MovesLeft <= 0)
            {
                _isGameEnded = true;
                Match3.EndGame(false).Forget();
                
                _statsManager.Stats.Losses++;
                _statsManager.Stats.WinLossRatio = _statsManager.Stats.Wins / (float) _statsManager.Stats.Losses;
            }
        }

        private void OnMatch(int matchCount)
        {
            MatchesLeft -= matchCount;
            _statsManager.Stats.Matches += matchCount;
            
            Score += matchCount * 10;
            _statsManager.Stats.Score += matchCount * 10;

            _hud.Setup(Score, MovesLeft, MatchesLeft);
            if (!_isGameEnded && MatchesLeft <= 0)
            {
                _isGameEnded = true;
                Match3.EndGame(true).Forget();
                
                _statsManager.Stats.Wins++;
                _statsManager.Stats.WinLossRatio = _statsManager.Stats.Wins / (float) _statsManager.Stats.Losses;
            }
        }
    }
}