using System;
using System.Windows;
using System.Windows.Shapes;
using AnimationLib;
using AnimationLib.AnimationDSL;
using AnimationLib.AnimationsCreator;
using AnimationLib.AnimationsCreator.MutliAnimation;
using GameLogic.Areas;
using GameLogic.Board;
using NoNameGame.BoardPresentation.Animations;
using NoNameGame.Configuration.Animations.AreaStateTransition;
using NoNameGame.Configuration.Animations.Periodic.Interfaces;
using NoNameGame.Controllers.GameLogic;
using NoNameGame.Models;

namespace NoNameGame.Configuration.Animations.Reset
{
    public class ResetAnimationFactory : IResetAnimationFactory
    {
        private readonly AreaStateTransitionsManager _areaStateTransitionsManager;
        private readonly ICurrentAnimationDelayProvider _animationDelayProvider;
        private readonly Func<BoardCoordinate, AreaModel> _areaCoordinateFunc;
        private readonly BoardSize _boardSize;
        public ResetAnimationFactory(AreaStateTransitionsManager areaStateTransitionsManager,
                                    ICurrentAnimationDelayProvider animationDelayProvider,
                                    Func<BoardCoordinate,AreaModel> areaCoordinateFunc,
                                    BoardSize boardSize           
            )
        {
            _areaStateTransitionsManager = areaStateTransitionsManager;
            _animationDelayProvider = animationDelayProvider;
            _areaCoordinateFunc = areaCoordinateFunc;
            _boardSize = boardSize;
        }
        public IGenericMultiAnimationCreator CreateAnimationCreator(Func<UIElement,BoardCoordinate> areaVisualisationCoordinateFunc)
        {
            return new GenericMultiAnimationCreator<BoardCoordinate>(areaVisualisationCoordinateFunc,
                bc => _animationDelayProvider.Get(_boardSize).ComputeDelay(bc.X, bc.Y),                    
                bc => _areaStateTransitionsManager.GetAnimationCreatorForArea(_areaCoordinateFunc(bc))
                );
        }
    }
}