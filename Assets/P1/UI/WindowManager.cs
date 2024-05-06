using System;
using System.Collections.Generic;
using UnityEngine;

namespace P1.UI
{
    public class WindowManager : MonoBehaviour
    {
        [SerializeField] private List<Window> _windows;
        private readonly Dictionary<Type, Window> _typeToWindow = new();

        private void Awake()
        {
            foreach (var window in _windows)
            {
                _typeToWindow.Add(window.GetType(), window);
            }
        }
        
        public T OpenWindow<T>() where T : Window
        {
            foreach (var window in _windows)
            {
                window.gameObject.SetActive(window.GetType() == typeof(T));
            }
            
            return (T) _typeToWindow[typeof(T)];
        }
    }
}