using System.Collections.Generic;
using System.Linq;

namespace NoNameGame.Controllers.Unlocks
{
    public class CompositeAchievment : IAchievement
    {
        private List<Achievement> achievements;
        public CompositeAchievment(List<Achievement> achievements)
        {
            this.achievements = achievements;
        }
        public void Execute()
        {
            foreach (var achievement in achievements)
            {
                achievement.Execute();
            }
        }
        public bool WasActionPerformed()
        {
            return achievements.Any(x => x.WasActionPerformed());
        }
    }
}