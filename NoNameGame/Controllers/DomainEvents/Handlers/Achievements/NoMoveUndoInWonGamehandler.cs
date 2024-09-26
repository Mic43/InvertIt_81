using System;
using Infrastructure.Storage;
using NoNameGame.Controllers.DomainEvents.Achievements;
using NoNameGame.Controllers.DomainEvents.Events;
using NoNameGame.Controllers.DomainEvents.Handlers.Base;
using NoNameGame.Controllers.DomainEvents.Helpers;
using NoNameGame.Controllers.DomainEvents.Infrastructure;

namespace NoNameGame.Controllers.DomainEvents.Handlers.Achievements
{
    public class NoMoveUndoInWonGameHandler : AchievementHandlerBase<GameWon,GameReset,GameLeft,MoveUndone>
    {
        private readonly IGameWonCondition _gameWonCondition;
        public bool WasUndoClicked
        {
             get
            {
                return
                    AppSettingsAccessor.GetValueOrDefault(
                        string.Format("NoMoveUndoInWonGameHandler{0}", Achievement.AchievementKey), false);
            }
            set
            {
                AppSettingsAccessor.AddOrUpdateValue(string.Format("NoMoveUndoInWonGameHandler{0}", Achievement.AchievementKey),value);
            }          
        }
        private void ResetUndoMoveListener()
        {
            WasUndoClicked = false;
        }
        private void ReportUndoMove()
        {
            WasUndoClicked = true;
        }
        public NoMoveUndoInWonGameHandler(IEventsBus eventsBus, NewAchievement achievement,IGameWonCondition gameWonCondition) : base(eventsBus, achievement)
        {
            if (gameWonCondition == null) throw new ArgumentNullException("gameWonCondition");
            _gameWonCondition = gameWonCondition;
        }
        public override void Handle(GameWon domainEvent)
        {
            if (!WasUndoClicked && _gameWonCondition.IsTrue(domainEvent))            
                Achievement.ReportProgress(1);            
            
            ResetUndoMoveListener();
        }
        public override void Handle(GameReset domainEvent)
        {
            ResetUndoMoveListener();
        }
        public override void Handle(GameLeft domainEvent)
        {
            ResetUndoMoveListener();
        }
        public override void Handle(MoveUndone domainEvent)
        {
            ReportUndoMove();
        }
    }
}