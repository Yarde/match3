using Common.Code.Model;
using Common.Common.Code;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;

namespace P2
{
    public class GameBoot : MonoBehaviour
    {
        [SerializeField] private BoardSettings _boardSettings;
        
        [Inject] private Match3 _match3;
        
        private void Start()
        {
            _match3.OnMatch += OnMatch;
            _match3.OnMove += OnMove;
            _match3.SetupBoard(_boardSettings).Forget();
        }

        private void OnMove()
        {
           Debug.Log("Move made");
        }

        private void OnMatch(int count)
        {
            Debug.Log("Matches count: " + count);
        }
    }
}