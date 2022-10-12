using UnityEngine;

namespace Code.Model
{
    public abstract class BoardElement : ScriptableObject
    {
        public Sprite sprite;
        public Vector2 size = Vector2.one;
        public bool isSwappable = true;

        public abstract void ApplyEffect();
    }
}