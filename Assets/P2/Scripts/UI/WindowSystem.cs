using System;
using VContainer;

namespace P2.UI
{
    public class WindowSystem
    {
        private readonly IObjectResolver _container;

        public WindowSystem(IObjectResolver container)
        {
            _container = container;
        }

        public void OpenWindow<T>() where T : ViewModel
        {
            var viewModel = Activator.CreateInstance<T>();
            _container.Inject(viewModel);
            viewModel.Show();
        }
    }
}