using System;
using System.Linq;
using Common.Code.Model;
using Common.Code.Model.Chips;
using Common.Code.Utils;
using UnityEngine;
using Random = System.Random;

namespace Common.Code.ChipGenerator
{
    public class RandomChipGeneratorBase : ChipGeneratorBase
    {
        [SerializeField] private RandomChipGeneratorData _data;
        private readonly Random _random = new();

        private float _probabilitySum;
        private float ProbabilitySum => _probabilitySum == 0 ? _probabilitySum = _data.chips.Sum(x => x.probablility) : _probabilitySum;

        public override BoardElement GetChip()
        {
            var value = _random.NextDouble() * ProbabilitySum;
            
            foreach (var chip in _data.chips)
            {
                value -= chip.probablility;

                if (!(value <= 0))
                    continue;
                
                return chip.chip;
            }
            
            return _data.chips.Random(_random).chip;
        }
    }
}