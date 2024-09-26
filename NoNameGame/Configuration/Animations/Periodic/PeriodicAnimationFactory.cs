using System;
using AnimationLib.AnimationsCreator.Interfaces;
using AnimationLib.AnimationsCreator.MutliAnimation;
using GameLogic.Board;
using NoNameGame.Configuration.Animations.Periodic.Interfaces;
using NoNameGame.Controllers.PeriodicAnimations;

namespace NoNameGame.Configuration.Animations.Periodic
{
    public class PeriodicAnimationFactory : IPeriodicAnimationFactory
    {
        private readonly IPeriodicAnimationTypeFactory _periodicAnimationTypeFactory;
        private readonly ICurrentAnimationDelayProvider _currentAnimationDelayProvider;

        public PeriodicAnimationFactory(IPeriodicAnimationTypeFactory periodicAnimationTypeFactory, ICurrentAnimationDelayProvider currentAnimationDelayProvider)
        {
            if (periodicAnimationTypeFactory == null) throw new ArgumentNullException("periodicAnimationTypeFactory");
            if (currentAnimationDelayProvider == null) throw new ArgumentNullException("currentAnimationDelayProvider");

            _periodicAnimationTypeFactory = periodicAnimationTypeFactory;
            _currentAnimationDelayProvider = currentAnimationDelayProvider;
        }
        public IMultiUIElementsAnimationCreator Create(BoardSize boardSize)
        {
            return new MultiAnimationCreator(_periodicAnimationTypeFactory.Create(), 
                _currentAnimationDelayProvider.Get(boardSize));
        }
    }
}