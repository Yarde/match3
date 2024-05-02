using System;
using System.Collections.Generic;
using Common.Code.Model.Chips;
using UnityEngine;

namespace Common.Code.Model
{
    [CreateAssetMenu]
    public class RandomChipGeneratorData : ScriptableObject
    {
        public List<GeneratorData> chips;
    }
    
    [Serializable]
    public struct GeneratorData
    {
        public BoardElement chip;
        public float probablility;
    }
}