using Common.Code.Model;
using Common.Common.Code;
using Cysharp.Threading.Tasks;
using P2.Objectives;
using UnityEngine;
using VContainer;

namespace P2
{
    public class GameBoot : MonoBehaviour
    {
        [SerializeField] private BoardSettings _boardSettings;

        [Inject] private Match3 _match3;
        [Inject] private ObjectivesSystem _objectivesSystem;

        private void Start()
        {
            _objectivesSystem.SetObjective(50, 15);
            _match3.StartGame(_boardSettings).Forget();
        }
    }
}