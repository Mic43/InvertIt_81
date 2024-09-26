namespace NoNameGame.Controllers.Unlocks
{
    public interface IAchievement
    {
        void Execute();
        bool WasActionPerformed();
    }
}