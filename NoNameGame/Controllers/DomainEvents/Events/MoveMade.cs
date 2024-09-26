using GameLogic.Board;
using NoNameGame.Controllers.DomainEvents.Infrastructure;
using NoNameGame.Models;

namespace NoNameGame.Controllers.DomainEvents.Events
{
    public class MoveMade : IDomainEvent
    {
        public BoardModel BeforeMoveBoard { get; private set; }
        public BoardModel AfterMoveBoard { get; private set; }

        public MoveMade(BoardModel beforeMoveBoard, BoardModel afterMoveBoard)
        {
            BeforeMoveBoard = beforeMoveBoard;
            AfterMoveBoard = afterMoveBoard;
        }
    }
}