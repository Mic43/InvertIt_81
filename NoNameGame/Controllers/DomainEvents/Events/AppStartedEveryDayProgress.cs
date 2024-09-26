using System;

namespace NoNameGame.Controllers.DomainEvents.Events
{
    internal class AppStartedEveryDayProgress : IDomainEvent
    {
        public int GoalDayStreakCount { get; private set; }
        public int CurrentDayStreakCount { get; private set; }

        public AppStartedEveryDayProgress(int goalDayStreakCount, int currentDayStreakCount)
        {
            if ( goalDayStreakCount < currentDayStreakCount)
                throw new ArgumentOutOfRangeException("GoalDayStreakCount value must be greater than CurrentDayStreakCount value!");

            GoalDayStreakCount = goalDayStreakCount;
            CurrentDayStreakCount = currentDayStreakCount;
        }
    }
}