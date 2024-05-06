using System;
using UnityEngine;

namespace P2.UI
{
    public abstract class View : MonoBehaviour, IDisposable
    {
        public abstract void Dispose();
    }
}