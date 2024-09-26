using System.Collections.Generic;

namespace NoNameGame.Controllers.Unlocks
{
    public interface ICompoisiteAchievementFactory
    {
        IAchievement CreateComposite(List<Achievement> achievements);
    }
}