using Common.Code.Model;
using Common.Common.Code;
using Cysharp.Threading.Tasks;
using P2.Objectives;
using P2.Scoring;
using P2.UI;
using UnityEngine;
using VContainer;

namespace P2
{
    public class GameBoot : MonoBehaviour
    {
        [SerializeField] private BoardSettings _boardSettings;

        [Inject] private Match3 _match3;
        [Inject] private WindowSystem _windowSystem;

        private void Start()
        {
            _windowSystem.OpenWindow<GameHUDViewModel>();
            _match3.StartGame(_boardSettings).Forget();
        }
    }
}