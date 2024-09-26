using NoNameGame.BoardPresentation.Animations;
using NoNameGame.Controllers.Themes;

namespace NoNameGame.Configuration.Animations.AreaStateTransition
{
    public interface IAreaStateTransitionManagerFactory
    {
        AreaStateTransitionsManager Create(Theme theme);
    }
}