using Common.Common.Code;
using P2.Achievements;
using P2.Gameplay;
using P2.Levels;
using P2.Network;
using P2.Objectives;
using P2.Rankings;
using P2.Scoring;
using P2.Stats;
using P2.UI;
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
            builder.Register<WindowSystem>(Lifetime.Singleton);
            builder.Register<GameplaySystem>(Lifetime.Singleton);
            builder.Register<LevelProgressionSystem>(Lifetime.Singleton);
            builder.Register<StatsSystem>(Lifetime.Singleton);
            builder.Register<AchievementsSystem>(Lifetime.Singleton);
            builder.Register<NetworkSystem>(Lifetime.Singleton);
            builder.Register<RankingsSystem>(Lifetime.Singleton);
        }
    }
}