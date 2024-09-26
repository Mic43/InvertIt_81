using System;
using System.Diagnostics;
using System.Windows.Media;
using Infrastructure.Storage;
using NoNameGame.Configuration.NewAchievements;

namespace NoNameGame.Controllers.DomainEvents.Achievements
{
    public class NewAchievement : INewAchievement
    {
        #region Properties
        public AchievementKey AchievementKey { get; private set; }

        public bool IsUnlocked { get; private set;    }

        public string Name { get; private set; }
        public string Description { get; private set; }

        public ImageSource ImageSource { get; private set; }
        public DateTime? DateAwarded { get; private set; }

        private readonly double _goalProgress;
        public double GoalProgress
        {
            get { return _goalProgress; }
        }
        /// <summary>
        /// Used when progress is not yet saved in persistent storage      
        /// </summary>
        private readonly double _defaultCurrentProgress;
        public double CurrentProgress { get; private set; }

        #endregion

        public event EventHandler Unlocked;
        public event EventHandler<AchievementProgressUpChangedArgs> ProgressUpChanged;


        private object _locker = new object();
        private readonly IValueStorer<AchievementKey,NewAchievementDto> _valueStorer;
        private void SetAsUnlocked()
        {
            CurrentProgress = GoalProgress;
            IsUnlocked = true;
            DateAwarded = DateTime.Today;
        }
        protected virtual void OnUnlocked()
        {
            var handler = Unlocked;
            if (handler != null) handler(this, EventArgs.Empty);
        }
        protected virtual void OnProgressChanged()
        {
            var handler = ProgressUpChanged;
            if (handler != null) handler(this, new AchievementProgressUpChangedArgs(CurrentProgress,GoalProgress));
        }


        private void ReadPersistedState()
        {
            var newAchievementDto = _valueStorer.Read(AchievementKey, new NewAchievementDto(AchievementKey, false, _defaultCurrentProgress,null));
            IsUnlocked = newAchievementDto.IsUnlocked;
            CurrentProgress = newAchievementDto.CurrentProgress;
            DateAwarded = newAchievementDto.DateAwarded;
        }
        public NewAchievement(IValueStorer<AchievementKey,NewAchievementDto> valueStorer, 
            AchievementKey id, string name,string description,
            double goalProgress, ImageSource imageSource,double defaultCurrentProgress = 0)
        {
            if (valueStorer == null) throw new ArgumentNullException("valueStorer");
            if (name == null) throw new ArgumentNullException("name");
            if (description == null) throw new ArgumentNullException("description");
            if (imageSource == null) throw new ArgumentNullException("imageSource");

            AchievementKey = id;
            Name = name;
            Description = description;
            ImageSource = imageSource;
            _valueStorer = valueStorer;           
            _goalProgress = goalProgress;
            _defaultCurrentProgress = defaultCurrentProgress;

            ReadPersistedState();
        }
        public void ReportProgress(double progressChange, bool silentUnlock = false)
        {
            Debug.WriteLine(string.Format("Report progress: change: {0} current: {1} goal: {2}", progressChange, CurrentProgress, GoalProgress));

            lock (_locker)
            {
                if (IsUnlocked || progressChange == 0.0) return;

                CurrentProgress += progressChange;
                if (CurrentProgress >= GoalProgress)
                {
                    SetAsUnlocked();
                }
                _valueStorer.Write(AchievementKey,
                    new NewAchievementDto(AchievementKey, IsUnlocked, CurrentProgress, DateAwarded));

                OnProgressChanged();

                if (IsUnlocked && !silentUnlock)
                    OnUnlocked();
            }
        }
        public void ResetProgress()
        {
            lock (_locker)
            {
                CurrentProgress = 0;
                IsUnlocked = false;
                DateAwarded = null;
            }
        }
    }
}
