using AnimationLib.AnimationsCreator.Interfaces;

namespace NoNameGame.Configuration.Animations.Periodic.Interfaces
{
    public interface IPeriodicAnimationTypeFactory
    {
        IUIElementAnimationCreator Create();
    }
}