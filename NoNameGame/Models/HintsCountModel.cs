namespace NoNameGame.Models
{
    public class HintsCountModel
    {
        public HintsCountModel(int currentHintsCount)
        {
            CurrentHintsCount = currentHintsCount;
        }
        public int CurrentHintsCount { get; private set; }
        public bool HintsAvailable { get { return CurrentHintsCount > 0; } }
    }
}