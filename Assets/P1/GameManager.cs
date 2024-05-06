using Common.Code.Model;
using Common.Common.Code;
using Cysharp.Threading.Tasks;
using P1.UI;
using UnityEngine;

namespace P1
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private BoardSettings _boardSettings;
        [SerializeField] private WindowManager _windowManager;

        private Match3 _match3;
        private GameHUD _hud;
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
            
            _movesLeft = _boardSettings.movesLimit;
            _matchesLeft = _boardSettings.matchesNeeded;

            _hud = _windowManager.OpenWindow<GameHUD>();
            _match3.StartGame(_boardSettings).Forget();
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
            Debug.Log("Game ended with " + (success ? "success" : "failure") + ", score: " + _score);
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
            Debug.Log("Move made");
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
            Debug.Log("Matched " + matchCount + " chips, " + _matchesLeft + " left");
            Debug.Log("Score: " + _score);
            if (!_isGameEnded && _matchesLeft <= 0)
            {
                _isGameEnded = true;
                _match3.EndGame(true).Forget();
            }
        }
    }
}