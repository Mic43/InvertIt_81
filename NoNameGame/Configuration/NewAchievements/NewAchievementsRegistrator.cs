using System;
using Autofac;
using Infrastructure.Storage;
using NoNameGame.Controllers.DomainEvents.Handlers;
using NoNameGame.Controllers.DomainEvents.Handlers.Achievements;
using NoNameGame.Controllers.DomainEvents.Handlers.Base;
using NoNameGame.Controllers.DomainEvents.Helpers;
using NoNameGame.Controllers.DomainEvents.Infrastructure;
using NoNameGame.Helpers.DateTime;
using NoNameGame.Levels;

namespace NoNameGame.Configuration.NewAchievements
{
    public class NewAchievementsRegistrator
    {
        private readonly NewAchievementsProvider _achievementsProvider;
        private readonly ILevelProvider _levelProvider;
        private readonly ILevelProgressStorer _levelProgressStorer;
        private NewAchievementsCollection _newAchievementsColletion;
        private ContainerBuilder _containerBuilder;

        private const int TutorialLevlGroupId = 3;
        private const int WarmUpLevlGroupId = 2;
        private const int EasyLevlGroupId = 6;
        private const int MediumLevlGroupId = 7;
        private const int HardLevlGroupId = 8;
        private const int EasyShapesLevlGroupId = 9;
        private const int MediumShapesLevlGroupId = 10;
        private const int HardShapesLevlGroupId = 11;
        private const int Hard9x9LevelGroupId = 18;
        private const int _levelPackId9X9 = 12;

        public NewAchievementsRegistrator(NewAchievementsProvider achievementsProvider, ILevelProvider levelProvider,
            ILevelProgressStorer levelProgressStorer)
        {
            if (achievementsProvider == null) throw new ArgumentNullException("achievementsProvider");
            if (levelProvider == null) throw new ArgumentNullException("levelProvider");
            if (levelProgressStorer == null) throw new ArgumentNullException("levelProgressStorer");
            _achievementsProvider = achievementsProvider;
            _levelProvider = levelProvider;
            _levelProgressStorer = levelProgressStorer;
        }
        public void Register(ContainerBuilder containerBuilder)
        {
            _containerBuilder = containerBuilder;
            _newAchievementsColletion = _achievementsProvider.Get();

            RegisterSingleAchievements(containerBuilder);

            RegisterLevelFinishedAchievement(containerBuilder, AchievementKey.TutorialFinished, TutorialLevlGroupId);
            RegisterLevelFinishedAchievement(containerBuilder, AchievementKey.WarmupFinished, WarmUpLevlGroupId);
            RegisterLevelFinishedAchievement(containerBuilder, AchievementKey.EasyFinished, EasyLevlGroupId);
            RegisterLevelFinishedAchievement(containerBuilder, AchievementKey.MediumFinished, MediumLevlGroupId);
            RegisterLevelFinishedAchievement(containerBuilder, AchievementKey.HardFinished, HardLevlGroupId);

            RegisterLevelFinishedAchievement(containerBuilder, AchievementKey.ShapesEasyFinished, EasyShapesLevlGroupId);
            RegisterLevelFinishedAchievement(containerBuilder, AchievementKey.ShapesMediumFinished, MediumShapesLevlGroupId);
            RegisterLevelFinishedAchievement(containerBuilder, AchievementKey.ShapesHardFinished, HardShapesLevlGroupId);

            RegisterPerfectSolveAchievement(containerBuilder, AchievementKey.PerfectCount1);
            RegisterPerfectSolveAchievement(containerBuilder, AchievementKey.PerfectCount2);
            RegisterPerfectSolveAchievement(containerBuilder, AchievementKey.PerfectCount3);

            RegisterWonGameStreakAchievement(containerBuilder, AchievementKey.WonGamesStreak1, EasyLevlGroupId);
            RegisterWonGameStreakAchievement(containerBuilder, AchievementKey.WonGamesStreak2, MediumLevlGroupId);
            RegisterWonGameStreakAchievement(containerBuilder, AchievementKey.WonGamesStreak3, HardLevlGroupId);

            RegisterGameTimeAchievement(containerBuilder, AchievementKey.GameTime1, EasyLevlGroupId,TimeSpan.FromSeconds(5));
            RegisterGameTimeAchievement(containerBuilder, AchievementKey.GameTime2, MediumLevlGroupId, TimeSpan.FromSeconds(7));
            RegisterGameTimeAchievement(containerBuilder, AchievementKey.GameTime3, HardLevlGroupId, TimeSpan.FromSeconds(10));           
        }
        private void RegisterSingleAchievements(ContainerBuilder containerBuilder)
        {
            containerBuilder.Register(
                context =>
                    new FacebookLikeHandler(context.Resolve<IEventsBus>(),
                        _newAchievementsColletion[AchievementKey.FbLike]))
                .AsImplementedInterfaces().SingleInstance();

            containerBuilder.Register(
                context =>
                    new AppRatedHandler(context.Resolve<IEventsBus>(),
                        _newAchievementsColletion[AchievementKey.RateApp]))
                .AsImplementedInterfaces().SingleInstance();

            containerBuilder.Register(
                context =>
                    new PlayerMoveCountHandler(context.Resolve<IEventsBus>(),
                        _newAchievementsColletion[AchievementKey.TotalMoveCount]))
                .AsImplementedInterfaces().SingleInstance();

            containerBuilder.Register(
                context =>
                    new TotalGameTimeInMinutesHandler(context.Resolve<IEventsBus>(),
                        _newAchievementsColletion[AchievementKey.TotalGameTime])).SingleInstance()
                .AsImplementedInterfaces();

            containerBuilder.Register(
                context =>
                    new InvertedAreasCountHandler(context.Resolve<IEventsBus>(),
                        _newAchievementsColletion[AchievementKey.TotalInvertCount])).SingleInstance()
                .AsImplementedInterfaces();

            containerBuilder.Register(
                context =>
                    new NoMoveUndoInWonGameHandler(context.Resolve<IEventsBus>(),
                        _newAchievementsColletion[AchievementKey.NoUndo],
                        new AllCondtitionsMet(new GameIsPerfect(), new PlayedLevelWasFirstTimeSolved(),
                            new PlayedLevelWasInLevelGroup(_levelProvider, Hard9x9LevelGroupId))))
                .SingleInstance()
                .AsImplementedInterfaces();

            containerBuilder.Register(
               context =>
                   new LevelPackFinishedHandler(context.Resolve<IEventsBus>(),
                       _newAchievementsColletion[AchievementKey.PackFinished9X9],_levelPackId9X9,_levelProvider,_levelProgressStorer))
               .SingleInstance()
               .AsImplementedInterfaces();

            containerBuilder.Register(
             context =>
                 new AppStartedEveryDayHandler(context.Resolve<IValueStorer<string,DateTime>>(),context.Resolve<IDateTimeNowProvider>(),
                     context.Resolve<IEventsBus>(),_newAchievementsColletion[AchievementKey.AppStartedEveryDayStreak]))
             .SingleInstance()
             .AsImplementedInterfaces();


        }
        private void RegisterWonGameStreakAchievement(ContainerBuilder containerBuilder, AchievementKey key,
            int levelGroupId)
        {
            int goalStreakCount = 5;
            containerBuilder.Register(
                context =>
                    new GameWonStreakHandler(context.Resolve<IEventsBus>(),
                        _newAchievementsColletion[key], goalStreakCount,
                         new AllCondtitionsMet(new GameIsPerfect(),new PlayedLevelWasInLevelGroup(_levelProvider, levelGroupId))))
                .SingleInstance()
                .AsImplementedInterfaces();
        }
        private void RegisterLevelFinishedAchievement(ContainerBuilder containerBuilder,
            AchievementKey key, int tutorialLevlGroupId)
        {
            containerBuilder.Register(
                context =>
                    new LevelGroupFinishedHandler(context.Resolve<IEventsBus>(),
                        _newAchievementsColletion[key], tutorialLevlGroupId, _levelProvider,
                        _levelProgressStorer))
                        .SingleInstance().AsImplementedInterfaces();
        }

        private void RegisterPerfectSolveAchievement(ContainerBuilder containerBuilder, AchievementKey key)
        {
            containerBuilder.Register(
                context =>
                    new GameWonConditionalHandler(context.Resolve<IEventsBus>(),
                        _newAchievementsColletion[key],
                        new AllCondtitionsMet(new GameIsPerfect(), new PlayedLevelWasFirstTimeSolved())))
                .SingleInstance()
                .AsImplementedInterfaces();
        }
        private void RegisterGameTimeAchievement(ContainerBuilder containerBuilder, AchievementKey achievementKey, int levelGroupId,TimeSpan timetoSolve)
        {
            containerBuilder.Register(
                context =>
                    new GameWonConditionalHandler(context.Resolve<IEventsBus>(),
                        _newAchievementsColletion[achievementKey],
                        new AllCondtitionsMet(new GameTimeLessThan(timetoSolve),
                            new PlayedLevelWasInLevelGroup(_levelProvider, levelGroupId))))
                .SingleInstance()
                .AsImplementedInterfaces();
        }
    }
}