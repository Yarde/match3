using System;
using System.Collections.Generic;
using System.Linq;
using Code.Model.Chips;
using Code.Utils;
using UnityEngine;
using Random = System.Random;

namespace Code.ChipGenerator
{
    public class RandomChipGeneratorBase : ChipGeneratorBase
    {
        [SerializeField] private List<GeneratorData> chips;
        private readonly Random _random = new();

        private float _probabilitySum;
        private float ProbabilitySum => _probabilitySum == 0 ? _probabilitySum = chips.Sum(x => x.probablility) : _probabilitySum;

        public override BoardElement GetChip()
        {
            var value = _random.NextDouble() * ProbabilitySum;
            
            foreach (var chip in chips)
            {
                value -= chip.probablility;

                if (!(value <= 0))
                    continue;
                
                return chip.chip;
            }
            
            return chips.Random(_random).chip;
        }
    }

    [Serializable]
    public struct GeneratorData
    {
        public BoardElement chip;
        public float probablility;
    }
}