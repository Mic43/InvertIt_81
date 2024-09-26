using GameLogic.Board;

namespace GameLogic.WinVerifiers
{
    public interface IWinVerifier
    {
        bool IsBoardWinning(AreaMatrix areaMatrix);
        AreaMatrix CreateWinningBoard(BoardSize boardSize);
    }

//    public class TestWinVerifier : IWinVerifier
//    {
//        public bool IsBoardWinning(AreaMatrix areaMatrix)
//        {
//            return CreateWinningBoard(areaMatrix.Size).Equals(areaMatrix);
//        }
//        public AreaMatrix CreateWinningBoard(BoardSize boardSize)
//        {
//            var areaMatrix = new AreaMatrix(boardSize);
//            for (int i = 0; i < boardSize; i++)
//            {
//                for (int j = 0; j < boardSize; j++)
//                {
//                    if (i==0 || j==0 || i ==boardSize -1 || j==boardSize -1)
//                        areaMatrix.InsertArea(false, i, j);
//                    else
//                        areaMatrix.InsertArea(true, i, j);
//                }
//            }
//            return areaMatrix;
//        }
//    }
}