namespace NoNameGame.Controllers.Unlocks.Conditions
{
    public class NullCondition : ICondition
    {
        public string GetDescription()
        {
            return string.Empty;
        }
        public bool IsTrue()
        {
            return false;
        }
    }
}