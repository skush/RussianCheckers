using System;
using System.Collections.Generic;
using System.Linq;

namespace Checkers.Model
{
    public class Board
    {
        protected List<Piece> pieces;
        public IEnumerable<Piece> Pieces
        {
            get { return pieces; }
        }

        public Board(IEnumerable<Piece> pieces)
        {
            this.pieces = pieces.ToList();
        }

        //create by cloning from another board
        public Board(Board board)
        {
            pieces = new List<Piece>();
            foreach(Piece p in board.pieces)
                pieces.Add(new Piece(p.Color, p.Coordinate));
        }

        public Board(Board board, Piece pieceToRemove) : this(board)
        {
            pieces.Remove(GetPieceByCoordinate(pieceToRemove.Coordinate));
        }

        public IEnumerable<Piece> GetPiecesByColor(PieceColor color)
        {
            return Pieces.Where(p => p.Color == color);
        }

        public Piece GetPieceByCoordinate(Square coordinate)
        {
            return Pieces.FirstOrDefault(p => p.Coordinate == coordinate);
        }

        public Square? GetSquare(Square start, Directions dir, int numOfSquaresToGo)
        {
            int x = start.X;
            int y = start.Y;
            switch (dir)
            {
                case Directions.FrontLeft:
                    x -= numOfSquaresToGo;
                    y += numOfSquaresToGo;
                    break;
                case Directions.FrontRight:
                    x += numOfSquaresToGo;
                    y += numOfSquaresToGo;
                    break;
                case Directions.BackLeft:
                    x -= numOfSquaresToGo;
                    y -= numOfSquaresToGo;
                    break;
                case Directions.BackRitht:
                    x += numOfSquaresToGo;
                    y -= numOfSquaresToGo;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("dir");
            }
            if (x >= 0 && x < Common.BoradLength && y >= 0 && y < Common.BoradLength)
            {
                return new Square {X = x, Y = y};
            }
            return null;
        }

        public override string ToString()
        {
            return string.Join("; ", pieces.OrderBy(p => p.Coordinate.ToString()));
        }

        public static Board ParseBoard(string input)
        {

            var pieces = new List<Piece>();
            pieces.AddRange(input.Split(';').Select(Piece.ParsePiece));
            return new Board(pieces);
        }
    }
}
