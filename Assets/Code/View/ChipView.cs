using System;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.View
{
    public class ChipView : MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private TextMeshProUGUI text;

        public void Setup(Sprite sprite, int i, int j)
        {
            image.sprite = sprite;
            text.text = $"{i},{j}";
        }

        public async UniTask Destroy()
        {
            await UniTask.Delay(1000);
            Destroy(gameObject);
        }
    }
}