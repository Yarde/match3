using System.Collections.Generic;
using Code.Model.Chips;
using Code.Utils;
using UnityEngine;
using Random = System.Random;

namespace Code.ChipGenerator
{
    public class RandomChipGeneratorBase : ChipGeneratorBase
    {
        [SerializeField] private List<BoardElement> chips;
        private readonly Random _random = new();

        public override BoardElement GetChip()
        {
            return chips.Random(_random);
        }
    }
}