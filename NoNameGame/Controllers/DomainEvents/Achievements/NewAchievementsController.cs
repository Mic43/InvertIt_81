using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;
using NoNameGame.Configuration.NewAchievements;

namespace NoNameGame.Controllers.DomainEvents.Achievements
{
    public class NewAchievementsController
    {
        private readonly NewAchievementsCollection _achievementsCollection;
        public NewAchievementsController(NewAchievementsCollection achievementsCollection)
        {
            _achievementsCollection = achievementsCollection;
        }
        public AchievementsModel GetAchievementsModel()
        {
            return new AchievementsModel(_achievementsCollection.Select(x=> 
                new SingleAchievementModel(x.Name,x.Description,x.ImageSource,x.IsUnlocked,x.DateAwarded,((int) x.GoalProgress),((int) x.CurrentProgress))));
        }
    }

    public class AchievementsModel
    {
        public ObservableCollection<SingleAchievementModel> Achievements { get; private set; }

        public AchievementsModel(IEnumerable<SingleAchievementModel> achievements)
        {
            Achievements = new ObservableCollection<SingleAchievementModel>(achievements);
        }
    }

    public class SingleAchievementModel
    {
        public string Name { get; private set; }
        public string Description { get; set; }
        public bool IsUnlocked { get; set; }
        public DateTime? UnlockDate { get; set; }
        public int GoalProgress { get; set; }
        public int CurrentProgress { get; set; }
        public bool IsSingleStep { get { return GoalProgress == 1; }}
        public ImageSource ImageSource { get; private set; }

        public SingleAchievementModel(string name, string description,ImageSource imageSource, bool isUnlocked, DateTime? unlockDate,int goalProgress,int currentProgress)
        {
            if (name == null) throw new ArgumentNullException("name");
            if (description == null) throw new ArgumentNullException("description");
            if (imageSource == null) throw new ArgumentNullException("imageSource");
            if ((unlockDate.HasValue && !isUnlocked)
                || (!unlockDate.HasValue && isUnlocked)) throw new ArgumentException("unlockDate");

            Name = name;
            Description = description;
            ImageSource = imageSource;
            IsUnlocked = isUnlocked;
            UnlockDate = unlockDate;
            GoalProgress = goalProgress;
            CurrentProgress = currentProgress;
        }
    }
}
