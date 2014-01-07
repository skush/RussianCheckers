using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkers.Model
{
    public class Move
    {
        protected Square start;
        public Square Start
        {
            get { return start; }
        }

        private Square end;
        public virtual Square End
        {
            get { return end; }
        }

        protected Move() { }

        public Move(Square start, Square end)
        {
            this.start = start;
            this.end = end;
        }

        public override string ToString()
        {
            return Start.ToString() + " - " + End.ToString();
        }
    }

    public class SequantialCapture : Move
    {
        public IList<Square> MoveSequance;

        public PieceColor Color;

        public IList<Piece> Captured;

        public SequantialCapture(Square start, IList<Square> sequence, Piece capturedPiece)
        {
            this.start = start;
            MoveSequance = sequence;
            Captured = new List<Piece>();
            Captured.Add(capturedPiece);
        }

        public SequantialCapture(SequantialCapture originalMove)
        {
            this.start = new Square(originalMove.Start.X, originalMove.Start.Y);
            var sequance = new List<Square>();
            foreach (var point in originalMove.MoveSequance)
                sequance.Add(point);
            this.MoveSequance = sequance;
            var captured = new List<Piece>();
            foreach (var cp in originalMove.Captured)
                captured.Add(cp);
            this.Captured = captured;
        }

        public override Square End
        {
            get
            {
                return MoveSequance.Last();
            }
        }

        public override string ToString()
        {
            // StringBuilder is not efficient here
            String res = Start.ToString();
            foreach (Square point in MoveSequance)
            {
                res += " - " + point.ToString();
            }
            return res;
        }
    }
}
