using System;

namespace NoNameGame.Controllers.DomainEvents.Achievements
{
    public class AchievementProgressUpChangedArgs
    {
        public double CurrentProgress { get; private set; }
        public double GoalProgress { get; private set; }

        public AchievementProgressUpChangedArgs(double currentProgress, double goalProgress)
        {
            if (goalProgress < currentProgress)
                throw new ArgumentOutOfRangeException("goalProgress must be greater or equal than currentProgress");
            CurrentProgress = currentProgress;
            GoalProgress = goalProgress;
        }
    }
}