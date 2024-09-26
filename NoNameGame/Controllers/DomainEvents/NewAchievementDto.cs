using System;
using System.Runtime.Serialization;
using NoNameGame.Configuration.NewAchievements;

namespace NoNameGame.Controllers.DomainEvents
{
    //WARNING!!!!
    //DO NOT CHANGE NAMESPACE OF NewAchievementDto class!!! or Any DTO class. Namespace is stored in settings in serialization.
    //Users updating app to new version with different namepsace cant use their old settings, which leads to overwrite their settings and game progress!!!


    [DataContract]
    public class NewAchievementDto
    {
        [DataMember]
        public AchievementKey AchievementKey { get; set; }
        [DataMember]
        public bool IsUnlocked { get; set; }
        [DataMember]
        public double CurrentProgress { get; set; }

        [DataMember]
        public DateTime? DateAwarded { get; set; }

        public NewAchievementDto(AchievementKey achievementKey, bool isUnlocked, double currentProgress, DateTime? dateAwarded)
        {
            AchievementKey = achievementKey;
            IsUnlocked = isUnlocked;
            CurrentProgress = currentProgress;
            DateAwarded = dateAwarded;
        }
    }
}