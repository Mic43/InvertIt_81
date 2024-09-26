using System;

namespace NoNameGame.Controllers.DomainEvents.Achievements
{
    public interface INewAchievement
    {
        event EventHandler Unlocked;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="progressChange"></param>
        /// <param name="silentUnlock">Do not rise Unlocked event</param>
        void ReportProgress(double progressChange,bool silentUnlock = false);
    }
}