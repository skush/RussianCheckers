using Checkers.Model.Exceptions;

namespace Checkers.Model
{
	/// <summary>
	/// a cell on the board
	/// </summary>
	public struct Square
	{
	    public int X;
	    public int Y;

		public static Square ParseCoordinates(string address)
		{
			if (address.Length != 2)
				throw new CheckersException("wrong address");
            string addressUpper = address.ToUpper();
			int x = addressUpper[0] - 'A';
			int y = addressUpper[1] - '1';
			if (y < 0 || y >= Common.BoradLength)
				throw new CheckersException("wrong address : 1st coordinate");
			if (x < 0 || x >= Common.BoradLength)
				throw new CheckersException("wrong address : 2nd coordinate");
			if ((x + y) % 2 != 0)
				throw new CheckersException("wrong address : not a black square");
            return new Square { X = x, Y = y };
        }

		public override string ToString()
		{
			return string.Format("{0}{1}", (char)('A' + X), Y + 1);
		}

        //TODO: implement ==, !=
        public bool Equals(Square other)
        {
            return other.X == X && other.Y == Y;
        }

        public static bool operator ==(Square left, Square right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Square left, Square right)
        {
            return !left.Equals(right);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (obj.GetType() != typeof(Square))
                return false;

            return Equals((Square)obj);
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode();
        }
	}
}
