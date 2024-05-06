using Common.Code.Model;
using Common.Common.Code;
using Cysharp.Threading.Tasks;
using P2.Gameplay;
using UnityEngine;
using VContainer;

namespace P2
{
    public class GameBoot : MonoBehaviour
    {
        [SerializeField] private BoardSettings _boardSettings;

        [Inject] private GameplaySystem _gameplaySystem;

        private void Start()
        {
            _gameplaySystem.StartGame(_boardSettings).Forget();
        }
    }
}