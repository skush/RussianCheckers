using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Checkers.Model.Exceptions;

namespace Checkers.Model
{
    // a shortcat - we skip AbstractPiece class to make it simple
    public class Piece
    {
        protected PieceColor color;
        public PieceColor Color
        {
            get { return color; }
        }

        protected Square coordinate;
        public Square Coordinate { get { return coordinate; } }
        
        public Piece(PieceColor color, Square square)
        {
            this.color = color;
            coordinate = square;
        }

        public static IEnumerable<Move> GetOneStepCaptureMoves(Piece piece, Board board)
        {
            List<Move> moves = new List<Move>();
            foreach (Directions dir in Enum.GetValues(typeof(Directions)))
            {
                Square dest = board.GetSquare(piece.Coordinate, dir, 2);
                if ((dest != null) && (board.GetPieceByCoordinate(dest) == null))
                {
                    Piece capture = board.GetPieceByCoordinate(board.GetSquare(piece.Coordinate, dir, 1));
                    if (capture != null && capture.Color != piece.Color)
                    {
                        moves.Add(new SequantialCapture(piece.Coordinate, new List<Square>(){ dest }));
                    }
                }
            }
            return moves;
        }

        /// <summary>
        /// gets simple one step capture moves - moves that kill opponent's pieces
        /// </summary>
        /// <param name="piece"></param>
        /// <param name="board"></param>
        /// <returns></returns>
        public static IEnumerable<Move> GetSingleCaptureMoves(Piece piece, Board board)
        {
            TempBoard tempBoard = new TempBoard(board, piece);
            return GetOneStepCaptureMoves(piece, tempBoard);
        }

        public static IEnumerable<Move> GetMultiStepCaptureMoves(Piece piece, Board board)
        {
            TempBoard tempBoard = new TempBoard(board, piece);
            List<Move> moves = new List<Move>();
            moves.AddRange(GetOneStepCaptureMoves(piece, tempBoard));
            return moves;
        }

        public static IEnumerable<Move> GetSimpleMoves(Piece piece, Board board)
        {
            TempBoard tempBoard = new TempBoard(board, piece);
            List<Move> moves = new List<Move>();
            IEnumerable<Directions> directions = DirectionUtils.GetDirections(piece.Color);
            foreach (Directions dir in directions)
            {
                Square dest = tempBoard.GetSquare(piece.Coordinate, dir, 1);
                if ((dest != null) && (tempBoard.GetPieceByCoordinate(dest) == null))
                {
                   moves.Add(new Move(piece.Coordinate, dest));
                }
            }
            return moves;
        }

        ////public static IEnumerable<Move> GetSingleCaptures(Board board, PieceColor colorOfNextMove)
        ////{
        ////    List<Move> moves = new List<Move>();
        ////    foreach (Piece piece in board.GetPiecesByColor(colorOfNextMove))
        ////    {
        ////        moves.AddRange(Piece.GetMultiStepCaptureMoves(piece, board));
        ////    }

        ////    if (moves.Count == 0)
        ////    {
        ////        foreach (Piece piece in board.GetPiecesByColor(colorOfNextMove))
        ////        {
        ////            moves.AddRange(Piece.GetSimpleMoves(piece, board));
        ////        }
        ////    }

        ////    return moves;
        ////}

        /// <summary>
        /// gets all pieces moves - none-Queens
        /// </summary>
        /// <param name="board"></param>
        /// <param name="colorOfNextMove"></param>
        /// <returns></returns>
        public static IEnumerable<Move> GetAllMoves(Board board, PieceColor colorOfNextMove)
        {
            List<Move> moves = new List<Move>();
            foreach (Piece piece in board.GetPiecesByColor(colorOfNextMove))
            {
                moves.AddRange(Piece.GetMultiStepCaptureMoves(piece, board));
            }

            if (moves.Count == 0)
            {
                foreach (Piece piece in board.GetPiecesByColor(colorOfNextMove))
                {
                    moves.AddRange(Piece.GetSimpleMoves(piece, board));
                }
            }

            return moves;
        }

        //protected virtual bool isQueen = false;
        protected bool isQueen = false;
        
        public bool IsQueen { get { return isQueen; } }

        public override string ToString()
        {
            return color.ToString() + ":" + coordinate.ToString();
        }

        public static PieceColor ParceColor(string strColor)
        {
            PieceColor color;
            switch (strColor.Substring(0, 1).ToUpper())
            {
                case "W":
                    color = PieceColor.White;
                    break;
                case "B":
                    color = PieceColor.Black;
                    break;
                default:
                    throw new CheckersException("incorrect input piece : color should be 'W' or 'B'");
            }
            return color;
        }

        public static Piece ParsePiece(string input)
        {
            if (input.Length != 3)
                throw new CheckersException("incorrect input piece : length should be 3");
            PieceColor color = ParceColor(input.Substring(0, 1).ToUpper());
            return new Piece(color, Square.ParseCoordinates(input.Substring(1,2).ToUpper()));
        }
    }

    public class Queen : Piece
    {
        public Queen(PieceColor color, Square square) : base(color, square) { isQueen = true; }

        Queen(Piece piece) : this(piece.Color, piece.Coordinate) {}

        //do not try to do it in for each
        public static Piece MakeQueen(Piece piece)
        {
            if (piece == null)
                throw new CheckersException("trying to construct Queen from uninitialized piece");
            return new Queen(piece);
        }

        public override string ToString()
        {
            return base.ToString() + ((isQueen)? " (Queen)" : string.Empty);
        }
    }

    //TODO: remove this fabric if not needed
    public static class PieceFabric
    {
        public static Piece CreatePiece(bool isQueen, PieceColor color, Square square)
        {
            if (isQueen)
                return new Queen(color, square);
            else
                return new Piece(color, square);
        }
    }
}
