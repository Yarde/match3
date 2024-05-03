using Common.Code.Model;
using Common.Common.Code;
using UnityEngine;

namespace P1
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private Match3 _match3;
        [SerializeField] private BoardSettings _boardSettings;

        private void Start()
        {
            _match3.SetupBoard(_boardSettings).Forget();
        }
    }
}