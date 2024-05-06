using System.Collections.Generic;
using Common.Code.Model;
using UnityEngine;

namespace P2.Levels
{
    [CreateAssetMenu]
    public class LevelList : ScriptableObject
    {
        public List<BoardSettings> _levels;
    }
}