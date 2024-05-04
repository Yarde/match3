using Common.Common.Code;
using P2.Objectives;
using P2.Scoring;
using VContainer;
using VContainer.Unity;

namespace P2.DependencyInjection
{
    public class ProjectContext : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<Match3>(Lifetime.Singleton);
            builder.Register<ScoringSystem>(Lifetime.Singleton);
            builder.Register<ObjectivesSystem>(Lifetime.Singleton);
            builder.Register<ObjectivesFactory>(Lifetime.Singleton);
        }
    }
}