using NoNameGame.Controllers.Unlocks.Actions;
using NoNameGame.Controllers.Unlocks.Conditions;
using NoNameGame.Helpers;

namespace NoNameGame.Controllers.Unlocks
{
    public class Achievement : IAchievement
    {
        public int Id { get; private set; }
        public AchievementType AchievementType { get; private set; }
        public bool IsEnabled { get; set; }
      
        public ICondition Condition { get; private set; }
        public IAction Action { get; private set; }
        private bool _actionPerformed;      

        public Achievement(int id,AchievementType type, ICondition condition, IAction action)
        {
            Id = id;
            AchievementType = type;
            Condition = condition;
            Action = action;
            IsEnabled = true;
        }
        public virtual bool WasActionPerformed()
        {
            return _actionPerformed;
        }
        public virtual void Execute()
        {
            _actionPerformed = false;
            if (IsEnabled)
            {
                if (Condition.IsTrue())
                {
                    Action.Perform();
                    _actionPerformed = true;
                }
            }
        }
    }

//    class UnlockAnimationAction : IAction
//    {
//        private readonly int _themeId;
//        public UnlockAnimationAction(int themeId)
//        {
//            _themeId = themeId;
//        }
//        public void Perform()
//        {
//            throw new System.NotImplementedException();
//        }
//    }
}