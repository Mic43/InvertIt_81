using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Input;
using Combinatorics.Collections;
using GameLogic.Board;

namespace GameLogic.MovesSequentionGenerators.DifficultyAssigners
{
    public interface IDifficultyAssigner
    {
        double Assign(ICollection<BoardCoordinate> generatedMoves);
    }

    public class ClusteringDifficultyAssigner : IDifficultyAssigner
    {
        private readonly Func<BoardCoordinate, BoardCoordinate, bool> _areConnectedFunc;
        public ClusteringDifficultyAssigner(Func<BoardCoordinate, BoardCoordinate, bool> areConnectedFunc)
        {
            _areConnectedFunc = areConnectedFunc;
        }
        public Func<BoardCoordinate, BoardCoordinate, bool> AreConnectedFunc
        {
            get { return _areConnectedFunc; }
        }
        public double Assign(ICollection<BoardCoordinate> generatedMoves)
        {
            var variations = new Variations<BoardCoordinate>(generatedMoves.ToList(), 2);
            var neigboursLookup = variations.Where(edge => _areConnectedFunc(edge[0],edge[1])).ToLookup(x => x[0], x => x[1]);

            List<float> coeffs = neigboursLookup.Select(
                x =>
                {
                    var max = (float) GetMaxPossibleCount(x);
                    if (max > 0)
                    {
                        List<int> enumerable = x.Select(y => neigboursLookup[x.Key].Count() -1).ToList();
                        var sum = enumerable.Sum();
                        return sum/max;
                    }
                    else // isloated vertice
                        return 0;
                })
                .ToList();
            if (!coeffs.Any())
                return 0.0;
            return coeffs.Average();
        }
        private int GetMaxPossibleCount(IEnumerable<BoardCoordinate> vertices)
        {
            int count = vertices.Count();
            return count *(count -1);
        }
    }
}