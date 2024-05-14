using P2.UI;
using P2.Windows;
using UnityEngine;
using VContainer;

namespace P2
{
    public class GameBoot : MonoBehaviour
    {
        [Inject] private WindowSystem _windowSystem;

        private void Start()
        {
            _windowSystem.Push<StartScreenViewModel>();
        }
    }
}