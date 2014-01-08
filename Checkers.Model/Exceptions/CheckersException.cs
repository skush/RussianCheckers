using System;

namespace Checkers.Model.Exceptions
{
	[Serializable]
	public class CheckersException : Exception
	{
		public CheckersException() { }

		public CheckersException(string message) : base(message) { }
	}
}
