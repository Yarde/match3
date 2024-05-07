using Cysharp.Threading.Tasks;
using P2.Gameplay;
using P2.Levels;
using UnityEngine;
using VContainer;

namespace P2
{
    public class GameBoot : MonoBehaviour
    {
        [Inject] private GameplaySystem _gameplaySystem;
        [Inject] private LevelProgressionSystem _levelProgressionSystem;

        private void Start()
        {
            var currentLevel = _levelProgressionSystem.CurrentLevel;
            _gameplaySystem.StartGame(currentLevel).Forget();
        }
    }
}