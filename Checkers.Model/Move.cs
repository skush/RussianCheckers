using System;
using System.Collections.Generic;
using System.Linq;

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
            return Start + " - " + End;
        }
    }

    public class SequantialCapture : Move
    {
        public IList<Square> MoveSequance;

        public IList<Piece> Captured;

        public SequantialCapture(Square start, IList<Square> sequence, Piece capturedPiece)
        {
            this.start = start;
            MoveSequance = sequence;
            Captured = new List<Piece> {capturedPiece};
        }

        public SequantialCapture(SequantialCapture originalMove)
        {
            start = originalMove.Start;
            MoveSequance = originalMove.MoveSequance.ToList();
            Captured = originalMove.Captured.ToList(); ;
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
            return MoveSequance.Aggregate(Start.ToString(), (current, point) => current + (" - " + point.ToString()));
        }
    }
}
