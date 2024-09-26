using NoNameGame.Controllers.Unlocks.Actions;
using NoNameGame.Controllers.Unlocks.Conditions;

namespace NoNameGame.Controllers.Unlocks
{
    public class ExecuteOnceAchievment : Achievement
    {      
        public ExecuteOnceAchievment(int id,AchievementType type, ICondition condition, IAction action) : base(id,type, condition, action)
        {
        }
        public override void Execute()
        {
            base.Execute();
            if (WasActionPerformed())
                IsEnabled = false;
        }
    }
}