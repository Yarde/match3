using Common.Common.Code;
using VContainer;
using VContainer.Unity;

namespace P2.DependencyInjection
{
    public class ProjectContext : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<Match3>(Lifetime.Singleton);
        }
    }
}