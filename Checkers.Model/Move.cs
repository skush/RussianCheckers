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
        public IEnumerable<Square> MoveSequance;

        public SequantialCapture(Square start, IEnumerable<Square> sequence)
        {
            this.start = start;
            MoveSequance = sequence;
        }

        public override Square End
        {
            get
            {
                return MoveSequance.Last();
            }
        }
    }
}
