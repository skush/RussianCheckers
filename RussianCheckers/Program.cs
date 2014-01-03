using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Checkers.Model;

namespace RussianCheckers
{
    class Program
    {
        static void Main(string[] args)
        {
            Board board;
            PieceColor nextMove;
            if (args.Length < 2)
            {
                //Console.WriteLine("Please provide pieces and color of next move as program parameters");
                board = Board.ParseBoard("wa1;bb2;bc3;wb4");
                nextMove = PieceColor.Black;
                ////board.MakeQueen("b2");
            }
            else
            {
                board = Board.ParseBoard(args[0]);
                nextMove = Piece.ParceColor(args[1]);
            }

            IEnumerable<Move> moves = Piece.GetAllMoves(board, nextMove);
            if (moves.Count() == 0)
                Console.WriteLine("No moves.");

            foreach (Move move in moves)
                Console.WriteLine(move);

            //Console.WriteLine(board);
            Console.ReadKey();
        }
    }
}
