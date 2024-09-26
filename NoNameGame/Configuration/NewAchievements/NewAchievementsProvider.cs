using System;
using System.Collections.Generic;
using System.Windows.Media.Imaging;
using Autofac;
using Infrastructure.Storage;
using NoNameGame.Controllers.DomainEvents;
using NoNameGame.Controllers.DomainEvents.Achievements;
using NoNameGame.Controllers.DomainEvents.Handlers;
using NoNameGame.Controllers.DomainEvents.Handlers.Base;
using NoNameGame.Controllers.DomainEvents.Infrastructure;
using NoNameGame.Resources;

namespace NoNameGame.Configuration.NewAchievements
{
    public sealed class NewAchievementsProvider
    {
        private readonly IValueStorer<AchievementKey, NewAchievementDto> _achievementprogresStorer;
        private readonly NewAchievementsCollection _achievementsCollection;
        public NewAchievementsProvider(IValueStorer<AchievementKey, NewAchievementDto> achievementprogresStorer)
        {
            if (achievementprogresStorer == null) throw new ArgumentNullException("achievementprogresStorer");
            _achievementprogresStorer = achievementprogresStorer;

            _achievementsCollection = new NewAchievementsCollection()
            {                
                new NewAchievement(_achievementprogresStorer, AchievementKey.FbLike,AppResources.Achievement_FacebookLike_Name,AppResources.Achievement_FacebookLike_Description, 1,CreateImageSourceForAchievement("Silverfb.png")),
                new NewAchievement(_achievementprogresStorer, AchievementKey.RateApp,AppResources.Achievement_RateApp_Name,AppResources.Achievement_RateApp_Description, 1,CreateImageSourceForAchievement("Silver_rate.png")),
                new NewAchievement(_achievementprogresStorer, AchievementKey.AppStartedEveryDayStreak,AppResources.Achievement_PlayEveryDay_Name,
                    string.Format(AppResources.Achievement_PlayEveryDay_Description, Constants.PlayEveryDayStreakLenght, Constants.FreeHintsForAppUsage)
                    ,Constants.PlayEveryDayStreakLenght,CreateImageSourceForAchievement("everyDay.png")),
                new NewAchievement(_achievementprogresStorer, AchievementKey.TotalMoveCount,AppResources.Achievement_TotalMoveCount_Name,AppResources.Achievement_TotalMoveCount, 500,CreateImageSourceForAchievement("star2_bronze.png")),
                new NewAchievement(_achievementprogresStorer, AchievementKey.TotalInvertCount,"Invertus Augustus",AppResources.Achievement_TotalInvertCount, 10000,CreateImageSourceForAchievement("star2_silver.png")),
                new NewAchievement(_achievementprogresStorer, AchievementKey.TotalGameTime,AppResources.Achievement_TotalGameTime_Name,AppResources.Achievement_TotalGameTime, 90,CreateImageSourceForAchievement("star2_gold.png")),
                new NewAchievement(_achievementprogresStorer, AchievementKey.NoUndo,AppResources.Achievement_NoUndo_Name,AppResources.AppResources_Achievement_NoUndo, 1,CreateImageSourceForAchievement("gold_god.png")),

                new NewAchievement(_achievementprogresStorer, AchievementKey.GameTime1,"Speedy gonzales",AppResources.Achievement_GameTime1, 1,CreateImageSourceForAchievement("sun_bronze2.png")),
                new NewAchievement(_achievementprogresStorer, AchievementKey.GameTime2,AppResources.Achievement_GameTime2_Name,AppResources.Achievement_GameTime2, 1,CreateImageSourceForAchievement("sun_silver2.png")),
                new NewAchievement(_achievementprogresStorer, AchievementKey.GameTime3,AppResources.Achievement_GameTime3_Name,AppResources.Achievement_GameTime3, 1,CreateImageSourceForAchievement("sun_gold2.png")),


                new NewAchievement(_achievementprogresStorer, AchievementKey.TutorialFinished,AppResources.Achievement_TutorialFinished_Name,AppResources.Achievement_TutorialFinished, 1,CreateImageSourceForAchievement("bronze.png")),
                new NewAchievement(_achievementprogresStorer, AchievementKey.WarmupFinished,AppResources.Achievement_WarmupFinished_Name,AppResources.Achievement_WarmupFinished, 1,CreateImageSourceForAchievement("silver.png")),
                new NewAchievement(_achievementprogresStorer, AchievementKey.EasyFinished,AppResources.Achievement_EasyFinished_Name,AppResources.Achievement_EasyFinished, 1,CreateImageSourceForAchievement("bronze2.png")),
                new NewAchievement(_achievementprogresStorer, AchievementKey.MediumFinished,AppResources.Achievement_MeiumFinished_Name,AppResources.Achievement_MediumFinished, 1,CreateImageSourceForAchievement("silver2.png")),
                new NewAchievement(_achievementprogresStorer, AchievementKey.HardFinished,AppResources.Achievement_HardFinished_Name,AppResources.Achievement_HardFinished, 1,CreateImageSourceForAchievement("gold2.png")),

                new NewAchievement(_achievementprogresStorer, AchievementKey.ShapesEasyFinished,AppResources.Achievement_ShapesEasyFinished_Name,AppResources.Achievement_ShapesEasyFinished_Description, 1,CreateImageSourceForAchievement("bronze3.png")),
                new NewAchievement(_achievementprogresStorer, AchievementKey.ShapesMediumFinished,AppResources.Achievement_ShapesMediumFinished_Name,AppResources.Achievement_ShapesMediumFinished_Decription, 1,CreateImageSourceForAchievement("silver3.png")),
                new NewAchievement(_achievementprogresStorer, AchievementKey.ShapesHardFinished,AppResources.Achievement_ShapesHardFinished_Name,AppResources.Achievement_ShapesHardFinished_Description, 1,CreateImageSourceForAchievement("gold3.png")),
                new NewAchievement(_achievementprogresStorer, AchievementKey.PackFinished9X9,AppResources.AppResources_Achievement_Ultra9X9Finished_Name,AppResources.Achievement_Ultra9X9Finished_Description, 1,CreateImageSourceForAchievement("gold_9x9.png")),

                new NewAchievement(_achievementprogresStorer, AchievementKey.PerfectCount1,AppResources.Achievement_PerfectCount1_Finished,AppResources.Achievement_PerfectCount1, 50,CreateImageSourceForAchievement("sun_bronze.png")),
                new NewAchievement(_achievementprogresStorer, AchievementKey.PerfectCount2,AppResources.Achievement_PerfectCount2_Finished,AppResources.Achievement_PerfectCount2, 100,CreateImageSourceForAchievement("sun_silver.png")),
                new NewAchievement(_achievementprogresStorer, AchievementKey.PerfectCount3,AppResources.Achievement_PerfectCount3_Finished,AppResources.Achievement_PerfectCount3, 150,CreateImageSourceForAchievement("sun_gold.png")),

                new NewAchievement(_achievementprogresStorer, AchievementKey.WonGamesStreak1,AppResources.Achievement_WonStreak1_Name,AppResources.Achievement_WonStreak1, 1,CreateImageSourceForAchievement("star_bronze.png")),
                new NewAchievement(_achievementprogresStorer, AchievementKey.WonGamesStreak2,AppResources.Achievement_WonStreak2_Name,AppResources.Achievement_WonStreak2, 1,CreateImageSourceForAchievement("star_silver.png")),
                new NewAchievement(_achievementprogresStorer, AchievementKey.WonGamesStreak3,AppResources.Achievement_WonStreak3_Name,AppResources.Achievement_WonStreak3, 1,CreateImageSourceForAchievement("star_gold.png")),                

                              
            };
        }
        private static BitmapImage CreateImageSourceForAchievement(string fileName)
        {
            return new BitmapImage(new Uri(@"/Assets/Achievements/small/" + fileName, UriKind.Relative));
        }
        public NewAchievementsCollection Get()
        {
            return _achievementsCollection;
        }
//        public IEnumerable<AchievementHandlerBaseBase> Get2(IEventsBus bus)
//        {
//            yield return
//                new TotalGameTimeInMinutesHandler(bus,
//                    new NewAchievement(_valueStorer, AchievementKey.TotalGameTime, "Napierdalacz czasu",
//                        "Graj przez minutę", 1));
//        }
    }
}