using Code.Model.Chips;
using UnityEngine;

namespace Code.ChipGenerator
{
    public abstract class ChipGeneratorBase : MonoBehaviour
    {
        public abstract BoardElement GetChip();
    }
}