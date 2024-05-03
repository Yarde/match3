using System.Collections.Generic;
using Common.Code.Model;
using Common.Code.Model.Chips;
using UnityEngine;

namespace Common.Code.ChipGenerator
{
    public abstract class ChipGeneratorBase : ScriptableObject
    {
        [SerializeField] public List<GeneratorData> chips;

        public abstract BoardElement GetChip();
    }
}