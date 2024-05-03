using System.Linq;
using Common.Code.Model.Chips;
using Common.Code.Utils;
using UnityEngine;
using Random = System.Random;

namespace Common.Code.ChipGenerator
{
    [CreateAssetMenu]
    public class RandomChipGenerator : ChipGeneratorBase
    {
        private readonly Random _random = new();

        private float _probabilitySum;
        private float ProbabilitySum => _probabilitySum == 0 
            ? _probabilitySum = chips.Sum(x => x.probablility) 
            : _probabilitySum;
        
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
}