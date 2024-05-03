using VContainer;
using VContainer.Unity;

namespace Yarde.DependencyInjection
{
    public class ProjectContext : LifetimeScope
    {

        protected override void Awake()
        {
            base.Awake();
            //Container.Resolve<GameplayFlow>().Start();
        }

        protected override void Configure(IContainerBuilder builder)
        {
            //builder.Register<SceneController>(Lifetime.Singleton);
            //builder.Register<GameplayFlow>(Lifetime.Singleton);
            //builder.RegisterInstance(_questline);
            //builder.RegisterComponentInHierarchy<AudioManager>();
        }
    }
}