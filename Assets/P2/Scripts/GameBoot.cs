using Common.Code.Model;
using Common.Common.Code;
using Cysharp.Threading.Tasks;
using P2.Objectives;
using P2.Scoring;
using UnityEngine;
using VContainer;

namespace P2
{
    public class GameBoot : MonoBehaviour
    {
        [SerializeField] private BoardSettings _boardSettings;

        [Inject] private Match3 _match3;
        [Inject] private ObjectivesSystem _objectivesSystem;
        [Inject] private ScoringSystem _scoringSystem;

        private void Start()
        {
            _match3.StartGame(_boardSettings).Forget();
        }
    }
}