using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{

    public struct SingleMove : IEquatable<SingleMove>

    {
        public int X { get; set; }
        public int Y { get; set; }
        public SingleMove(int x, int y) : this()
        {
            X = x;
            Y = y;
        }

        public bool Equals(SingleMove bc)
        {
            return (X == bc.X) && (Y == bc.Y);
        }
        public override bool Equals(Object obj)
        {
            // Check for null values and compare run-time types.
            if (obj == null || GetType() != obj.GetType())
                return false;

            var bc = (SingleMove)obj;
            return Equals(bc);
        }
        public override int GetHashCode()
        {
            return X ^ Y;
        }


    }
    public static class MovesCollectionFormatter
    {
        public static string SerializeMovesCollection(List<SingleMove> list)
        {
            return list.Aggregate(new StringBuilder(),
                (sb, bc) => sb.Append(string.Format("({0},{1})", bc.X, bc.Y)))
                .ToString();
        }
        public static List<SingleMove> ParseString(string s)
        {
            if (string.IsNullOrEmpty(s))
                return Enumerable.Empty<SingleMove>().ToList();
            string[] strings = s.Split(new char[] { '(' });

            return strings.Skip(1).Select(x => new SingleMove(int.Parse(x[0].ToString()), int.Parse(x[2].ToString()))).ToList();
        }
    }
}
