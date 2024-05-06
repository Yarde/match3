using Cysharp.Threading.Tasks;
using P2.Gameplay;
using P2.Levels;
using P2.Progression;
using UnityEngine;
using VContainer;

namespace P2
{
    public class GameBoot : MonoBehaviour
    {
        [Inject] private GameplaySystem _gameplaySystem;
        [Inject] private ProgressionSystem _progressionSystem;
        [Inject] private LevelSystem _levelSystem;

        private void Start()
        {
            
            var currentLevel = _levelSystem.CurrentLevel;
            _gameplaySystem.StartGame(currentLevel).Forget();
        }
    }
}