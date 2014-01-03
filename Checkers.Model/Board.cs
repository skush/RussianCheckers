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

        ////protected Board() { }

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

        public IEnumerable<Piece> GetPiecesByColor(PieceColor color)
        {
            return Pieces.Where(p => p.Color == color);
        }

        public Piece GetPieceByCoordinate(Square coordinate)
        {
            return Pieces.FirstOrDefault(p => p.Coordinate.X == coordinate.X && p.Coordinate.Y == coordinate.Y);
        }

        public Square GetSquare(Square start, Directions dir, int numOfSquaresToGo)
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
                    throw new ArgumentOutOfRangeException("enum Direction, argument dir");
            }
            if (x >= 0 && x < Common.BoradLength && y >= 0 && y < Common.BoradLength)
                return new Square(x, y);
            return null;
        }

        public void MakeQueen(Square coordinate)
        {
            Piece piece = GetPieceByCoordinate(coordinate);
            Piece queen = Queen.MakeQueen(piece);
            pieces.Remove(piece);
            pieces.Add(queen);
        }

        public void MakeQueen(string coordinate)
        {
            MakeQueen(Square.ParseCoordinates(coordinate));
        }

        public override string ToString()
        {
            return string.Join("\n", pieces.OrderBy(p => p.Coordinate.ToString()));
        }

        public static Board ParseBoard(string input)
        {
            List<Piece> pieces = new List<Piece>();
            string[] stringPieces = input.Split(';');
            foreach (var p in stringPieces)
            {
                Piece newPiece = Piece.ParsePiece(p);
                pieces.Add(newPiece);
            }
            return new Board(pieces);
        }
    }
}
