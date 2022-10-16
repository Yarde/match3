using UnityEngine;

namespace Code.Model.Chips
{
    public abstract class BoardElement : ScriptableObject
    {
        public Sprite sprite;
        public bool isSwappable;
        public bool isClickable;

        public abstract void ApplyEffect();
    }
}