using Code.Model.Chips;
using UnityEngine;

namespace Code.Model
{
    [CreateAssetMenu]
    public class SimpleColorChip : BoardElement
    {
        public Color color;
        public ParticleSystem particleSystem;
        
        public override void ApplyEffect()
        {
            //particleSystem.Play();
            // play some particles and destroy
        }
    }
}