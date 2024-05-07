using System;
using System.Collections.Generic;
using System.Linq;
using VContainer;

namespace P2.UI
{
    public class WindowSystem
    {
        private readonly List<ViewModel> _windowsStack = new();
        private readonly IObjectResolver _container;

        public WindowSystem(IObjectResolver container)
        {
            _container = container;
        }

        public T Push<T>() where T : ViewModel
        {
            var window = Activator.CreateInstance<T>();
            _container.Inject(window);
            window.Setup();
            
            foreach (var w in _windowsStack)
            {
                w.Hide();
            }
            
            _windowsStack.Add(window);
            
            return window;
        }
        
        public void Pop()
        {
            var window = _windowsStack.Last();
            _windowsStack.Remove(window);
            window.Close();
            
            if (_windowsStack.Count > 0)
            {
                _windowsStack.Last().Show();
            }
        }
    }
}