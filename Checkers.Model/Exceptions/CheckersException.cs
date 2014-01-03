using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkers.Model.Exceptions
{
	[Serializable]
	public class CheckersException : Exception
	{
		public CheckersException() { }

		public CheckersException(string message) : base(message) { }
	}
}
