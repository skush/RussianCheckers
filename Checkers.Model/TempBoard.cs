using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkers.Model
{
    /// <summary>
    /// is used in calculations of posible moves
    /// </summary>
    class TempBoard : Board
    {
        //public IEnumerable<Piece> Captured;

        public TempBoard(Board board, Piece piece) : base(board)
        {
            pieces.Remove(piece);
            //Captured = new List<Piece>();
        }
    }
}
