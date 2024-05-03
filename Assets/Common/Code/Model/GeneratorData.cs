using System;
using Common.Code.Model.Chips;

namespace Common.Code.Model
{
    [Serializable]
    public struct GeneratorData
    {
        public BoardElement chip;
        public float probablility;
    }
}