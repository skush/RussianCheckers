using Checkers.Model.Exceptions;
using Checkers.Model;

namespace Checkers.Model
{
	/// <summary>
	/// a cell on the board
	/// </summary>
	public class Square
	{
		private int x;
		private int y;

		public Square(int x, int y)
		{
			this.x = x;
			this.y = y;
		}

        public int X { get { return x; } }
        public int Y { get { return y; } }

        //public Square(string address) { ... }
		public static Square ParseCoordinates(string address)
		{
			if (address.Length != 2)
				throw new CheckersException("wrong address");
            string addressUpper = address.ToUpper();
			int x = addressUpper[0] - 'A';
			int y = addressUpper[1] - '1';
			if (y < 0 || y >= Common.BoradLength)
				throw new CheckersException("wrong address : 1st coordinate");
			else if (x < 0 || x >= Common.BoradLength)
				throw new CheckersException("wrong address : 2nd coordinate");
			else if ((x + y) % 2 != 0)
				throw new CheckersException("wrong address : not a black square");
			return new Square(x, y);
		}

		public override string ToString()
		{
			return string.Format("{0}{1}", (char)('A' + x), y + 1);
		}

        //TODO: implement ==, !=
        ////public bool Equals(Square other)
        ////{
        ////    return other.x == x && other.y == y;
        ////}

        ////public static bool operator ==(Square left, Square right)
        ////{
        ////    if (left == null)
        ////        return false;
        ////    return left.Equals(right);
        ////}

        ////public static bool operator !=(Square left, Square right)
        ////{
        ////    if (left == null)
        ////        return false;
        ////    return !left.Equals(right);
        ////}

        ////public override bool Equals(object obj)
        ////{
        ////    if (ReferenceEquals(null, obj))
        ////        return false;
        ////    if (obj.GetType() != typeof(Square))
        ////        return false;

        ////    return Equals((Square)obj);
        ////}

        ////public override int GetHashCode()
        ////{
        ////    return x.GetHashCode() ^ y.GetHashCode();
        ////}
	}
}
