using System.Collections.Generic;
using NoNameGame.Controllers.Unlocks;

namespace NoNameGame.Configuration.Achievements
{
    public interface IAchievementsCreator
    {
        List<Achievement> Get();
    }
}