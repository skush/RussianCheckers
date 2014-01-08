using System;
using System.Collections.Generic;
using System.Linq;
using Checkers.Model.Exceptions;

namespace Checkers.Model
{
    public class Piece
    {
        private PieceColor color;
        public PieceColor Color
        {
            get { return color; }
        }

        private Square coordinate;
        public Square Coordinate { get { return coordinate; } }

        private bool isQueen;
        public bool IsQueen { get { return isQueen; } }

        public Piece(PieceColor color, Square square) : this(color, square, false) { }
        
        public Piece(PieceColor color, Square square, bool isQueen)
        {
            this.color = color;
            this.coordinate = square;
            this.isQueen = isQueen;
        }

        private static List<SequantialCapture> GetCaptureMovesRecursive(Piece piece, Board board, SequantialCapture oriMove)
        {
            List<SequantialCapture> moves = new List<SequantialCapture>();
            foreach (Directions dir in Enum.GetValues(typeof(Directions)))
            {
                Square start = (oriMove != null) ? oriMove.End : piece.Coordinate;
                Square? dest = board.GetSquare(start, dir, 2);

                if ((dest.HasValue)
                    && (board.GetPieceByCoordinate(dest.Value) == null
                    //piece can still return back to its square
                    || (dest.Value == piece.Coordinate)))
                {
                    Piece capture = board.GetPieceByCoordinate(board.GetSquare(start, dir, 1).Value);
                    if (capture != null && capture.Color != piece.Color)
                    {
                        SequantialCapture newMove = null;
                        if (oriMove != null)
                        {
                            if(!oriMove.Captured.Any(cp => cp.Coordinate == capture.Coordinate))
                            {
                                newMove = new SequantialCapture(oriMove);
                                newMove.MoveSequance.Add(dest.Value);
                                newMove.Captured.Add(capture);
                            }
                        }
                        else
                        {
                            newMove = new SequantialCapture(start, new List<Square> { dest.Value }, capture);
                        }
                        if (newMove != null)
                        {
                            var moves2Add = GetCaptureMovesRecursive(piece, board, newMove);
                            if (moves2Add != null)
                                moves.AddRange(moves2Add);
                        }
                    }
                }
            }
            if (moves.Any())
                return moves;
            return oriMove != null ? new List<SequantialCapture>() { oriMove } : new List<SequantialCapture>();
        }

        private static IEnumerable<Move> GetSimpleMoves(Piece piece, Board board)
        {
            var moves = new List<Move>();
            var directions = DirectionUtils.GetDirections(piece.Color);

            foreach (Directions dir in directions)
            {
                Square? dest = board.GetSquare(piece.Coordinate, dir, 1);
                if ((dest != null) && (board.GetPieceByCoordinate(dest.Value) == null))
                {
                    moves.Add(new Move(piece.Coordinate, dest.Value));
                }
            }
            return moves;
        }

        /// <summary>
        /// gets all pieces moves - none-Queens
        /// </summary>
        /// <param name="board"></param>
        /// <param name="colorOfNextMove"></param>
        /// <returns></returns>
        public static IEnumerable<Move> GetAllMoves(Board board, PieceColor colorOfNextMove)
        {
            var moves = new List<Move>();
            foreach (Piece piece in board.GetPiecesByColor(colorOfNextMove))
            {
                moves.AddRange(GetCaptureMovesRecursive(piece, board, null));
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

        public override string ToString()
        {
            return color + ":" + coordinate;
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
}
