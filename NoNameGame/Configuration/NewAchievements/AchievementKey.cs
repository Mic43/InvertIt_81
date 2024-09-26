using System.Runtime.Serialization;

namespace NoNameGame.Configuration.NewAchievements
{
    [DataContract]
    public enum AchievementKey
    {
        [EnumMember] TotalMoveCount,
        [EnumMember] TotalGameTime,
        [EnumMember] WonGamesStreak1,
        [EnumMember] WonGamesStreak2,
        [EnumMember] WonGamesStreak3,
        [EnumMember] TutorialFinished,
        [EnumMember] WarmupFinished,
        [EnumMember] EasyFinished,
        [EnumMember] MediumFinished,
        [EnumMember] HardFinished,
        [EnumMember] TotalInvertCount,
        [EnumMember] PerfectCount1,
        [EnumMember] PerfectCount2,
        [EnumMember] PerfectCount3,
        [EnumMember] GameTime1,
        [EnumMember] GameTime2,
        [EnumMember] GameTime3,
        [EnumMember] ShapesEasyFinished,
        [EnumMember] ShapesMediumFinished,
        [EnumMember] ShapesHardFinished,
        [EnumMember] FbLike,
        [EnumMember] RateApp,
        [EnumMember] NoUndo,
        [EnumMember] PackFinished9X9,
        [EnumMember]
        AppStartedEveryDayStreak
    }
}