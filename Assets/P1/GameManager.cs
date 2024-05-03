using Common.Code.Model;
using Common.Common.Code;
using UnityEngine;

namespace P1
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private BoardSettings _boardSettings;
        
        private Match3 _match3;

        private void Start()
        {
            _match3 = new Match3();
            _match3.SetupBoard(_boardSettings).Forget();
        }
    }
}