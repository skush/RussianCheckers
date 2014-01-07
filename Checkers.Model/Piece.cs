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

        protected bool isQueen;
        public bool IsQueen { get { return isQueen; } }

        public Piece(PieceColor color, Square square) : this(color, square, false) { }
        
        public Piece(PieceColor color, Square square, bool isQueen)
        {
            this.color = color;
            this.coordinate = square;
            this.isQueen = isQueen;
        }

        public static List<Move> GetOneStepCaptureMoves(Piece piece, Board board, IEnumerable<Piece> capturedPieces)
        {
            List<Move> moves = new List<Move>();
            foreach (Directions dir in Enum.GetValues(typeof(Directions)))
            {
                Square dest = board.GetSquare(piece.Coordinate, dir, 2);
                if ((dest != null) && (board.GetPieceByCoordinate(dest) == null))
                {
                    Piece capture = board.GetPieceByCoordinate(board.GetSquare(piece.Coordinate, dir, 1));
                    if (capture != null && capture.Color != piece.Color && !capturedPieces.Any(cp => cp.Coordinate == capture.Coordinate))
                    {
                        moves.Add(new SequantialCapture(piece.Coordinate, new List<Square>() { new Square(dest.X, dest.Y) }, capture));
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
            Board tempBoard = new Board(board, piece);
            return GetOneStepCaptureMoves(piece, tempBoard, new List<Piece>());
        }

        //
        public static IEnumerable<SequantialCapture> GetCaptureMovesRecursive(List<SequantialCapture> moves, Board tempBoard)
        {
            List<SequantialCapture> extMoves = new List<SequantialCapture>();
            foreach (var move in moves)
            {
                var piece = new Piece(move.Color, move.End);
                var newMoves = GetOneStepCaptureMoves(piece, tempBoard, move.Captured);
                if (newMoves.Count() == 0)
                {
                    extMoves.Add(move);
                }
                else
                {
                    foreach (var m in newMoves)
                    {
                        var newMove = new SequantialCapture(move);
                        newMove.MoveSequance.Add(m.End);
                        extMoves.Add(newMove);
                    }
                }
            }
            return extMoves;
        }

        public static IEnumerable<Move> GetMultiStepCaptureMoves(Piece piece, Board board)
        {
            Board tempBoard = new Board(board, piece);
            IEnumerable<SequantialCapture> initialMoves = GetOneStepCaptureMoves(piece, tempBoard, new List<Piece>()).Cast<SequantialCapture>();
            IEnumerable<SequantialCapture> moves = GetCaptureMovesRecursive(initialMoves.ToList(), tempBoard);
            return moves;
        }

        public static IEnumerable<Move> GetSimpleMoves(Piece piece, Board board)
        {
            Board tempBoard = new Board(board, piece);
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
}
