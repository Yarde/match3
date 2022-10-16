using UnityEngine;
using UnityEngine.UI;

namespace Code.View
{
    public class ChipView : MonoBehaviour
    {
        [SerializeField] private Image image;

        public void Setup(Sprite sprite)
        {
            image.sprite = sprite;
        }
    }
}