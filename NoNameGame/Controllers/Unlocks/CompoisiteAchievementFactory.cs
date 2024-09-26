using System.Collections.Generic;

namespace NoNameGame.Controllers.Unlocks
{
    public class CompoisiteAchievementFactory : ICompoisiteAchievementFactory
    {
        public IAchievement CreateComposite(List<Achievement> achievements)
        {
            return new CompositeAchievment(achievements);
        }
    }
}