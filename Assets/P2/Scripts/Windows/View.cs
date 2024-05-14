using System;
using UnityEngine;

namespace P2.Windows
{
    public abstract class View : MonoBehaviour, IDisposable
    {
        public abstract void Dispose();
    }
}