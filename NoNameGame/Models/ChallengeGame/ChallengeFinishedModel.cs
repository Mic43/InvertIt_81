using GameLogic.Board;

namespace NoNameGame.Models.ChallengeGame
{
    public class ChallengeFinishedModel
    {
        public BoardCoordinate PlayerLastMove { get; private set; }

        public ChallengeFinishedModel(BoardCoordinate playerLastMove)
        {
            PlayerLastMove = playerLastMove;
        }
    }
}