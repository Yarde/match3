using Common.Code.Model;
using Common.Common.Code;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace P1
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private BoardSettings _boardSettings;
        
        private Match3 _match3;
        private int _movesLeft;
        private int _matchesLeft;

        private void Start()
        {
            _match3 = new Match3();
            
            _match3.OnMatch += OnMatch;
            _match3.OnMove += OnMove;
            
            _movesLeft = _boardSettings.movesLimit;
            _matchesLeft = _boardSettings.matchesNeeded;
            _match3.StartGame(_boardSettings).Forget();
        }
        
        private void OnDestroy()
        {
            _match3.OnMatch -= OnMatch;
            _match3.OnMove -= OnMove;
        }

        private void OnMove()
        {
            Debug.Log("Move made");
            _movesLeft--;
            if (_movesLeft <= 0)
            {
                Debug.Log("Lose");
                _match3.EndGame();
            }
        }

        private void OnMatch(int matchCount)
        {
            _matchesLeft -= matchCount;
            Debug.Log("Matched " + matchCount + " chips, " + _matchesLeft + " left");
            if (_matchesLeft <= 0)
            {
                Debug.Log("Win");
                _match3.EndGame();
            }
        }
    }
}