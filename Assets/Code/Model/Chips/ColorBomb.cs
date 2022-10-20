using UnityEngine;

namespace Code.Model.Chips
{
    [CreateAssetMenu]
    public class ColorBomb : SimpleColorChip
    {
        public override void ApplyEffect()
        {
            // destroy all chips of it's color
            
            base.ApplyEffect();
        }
    }
}