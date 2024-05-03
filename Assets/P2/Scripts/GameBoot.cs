using Common.Code.Model;
using Common.Common.Code;
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
            _match3.SetupBoard(_boardSettings).Forget();
        }
    }
}