using System;
using System.Windows.Navigation;
using NoNameGame.Controllers.DomainEvents.Events;
using NoNameGame.Controllers.DomainEvents.Handlers.Base;
using NoNameGame.Controllers.Hints.HintsCount;

namespace NoNameGame.Controllers.DomainEvents.Handlers.PurchaseHintSuggest
{
    public class PlayerMadeManyMoves : 
        IDomainEventHandler<MoveMade>,
        IDomainEventHandler<GameWon>,
        IDomainEventHandler<GameLeft>,
         IDomainEventHandler<GameReset>,
        IDomainEventHandler<NewGameStarted>
    {
        private readonly IPurchaseHintsSuggester _purchaseHintsSuggester;
        private readonly IHintsCountProvider _hintsCountProvider;
        private bool _wasSuggestionAlreadyMade = false;
        private int _movesMadeInCurrentLevel = 0;
        private int _perfectSolveMovesCount = 0;
        private const float Coefficient = 2f;

        private bool ShouldMakeSuggestion()
        {
            return !_wasSuggestionAlreadyMade && _movesMadeInCurrentLevel > _perfectSolveMovesCount*Coefficient;
        }
        private void ResetCounter()
        {
            _wasSuggestionAlreadyMade = false;
            _movesMadeInCurrentLevel = 0;
        }
        public PlayerMadeManyMoves(IPurchaseHintsSuggester purchaseHintsSuggester,IHintsCountProvider hintsCountProvider)
        {
            if (purchaseHintsSuggester == null) throw new ArgumentNullException("purchaseHintsSuggester");
            if (hintsCountProvider == null) throw new ArgumentNullException("hintsCountProvider");
            _purchaseHintsSuggester = purchaseHintsSuggester;
            _hintsCountProvider = hintsCountProvider;
        }
        public void Handle(MoveMade domainEvent)
        {
            _movesMadeInCurrentLevel++;

            if (ShouldMakeSuggestion())
            {
                _purchaseHintsSuggester.Suggest();
                _wasSuggestionAlreadyMade = true;
            }
        }
//        private bool ShouldMakeSuggestion()
//        {
//            return !_wasSuggestionAlreadyMade &&  _movesMadeInCurrentLevel > _perfectSolveMovesCount * Coefficient && _hintsCountProvider.Get() == 0;
//        }

        public void Handle(GameWon domainEvent)
        {
            ResetCounter();
        }
        public void Handle(GameLeft domainEvent)
        {
            ResetCounter();
        }
        public void Handle(NewGameStarted domainEvent)
        {
            _perfectSolveMovesCount = domainEvent.LevelToPlay.WinningMovesSequention.Count;
        }
        public void Handle(GameReset domainEvent)
        {
            ResetCounter();
        }
    }
}