using NoNameGame.Levels;

namespace NoNameGame.Controllers.Unlocks.Conditions
{
    public class LevelGroupFinished : ICondition
    {
        private readonly int _levelGroupId;
        private readonly ILevelProgressStorer _levelProgressStorer;
        public LevelGroupFinished(int levelGroupId,ILevelProgressStorer levelProgressStorer)
        {
            _levelGroupId = levelGroupId;
            _levelProgressStorer = levelProgressStorer;
        }
        public string GetDescription()
        {
            return string.Format("Level group {0} was finished", _levelGroupId);
        }
        public bool IsTrue()
        {
            return false;
            //_levelProgressStorer.
        }
    }
}