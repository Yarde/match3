using Common.Code.Model.Chips;
using UnityEngine;

namespace Common.Code.ChipGenerator
{
    public abstract class ChipGeneratorBase : MonoBehaviour
    {
        public abstract BoardElement GetChip();
    }
}