using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Checkers.Model.Test
{
    [TestClass]
    public class IntegrationTest
    {
        enum TestBoardTypes
        {
            Simple,
            MultiStep,
            Void
        }

        private class IntegrationTestResult
        {
            public int sourseBoardNumber;
            public TestBoardTypes TestType;
            public PieceColor NextMove;
            public List<String> moves;
        }

        static Dictionary<TestBoardTypes, List<Board>> boards;

        static List<IntegrationTestResult> expectedMoves;

        [ClassInitialize()]
        public static void IntegrationTestInit(TestContext context)
        {
            boards = new Dictionary<TestBoardTypes, List<Board>>();
            boards.Add(TestBoardTypes.Simple, new List<Board>{
                Board.ParseBoard("wa1;bb2;bc3;wb4"),
                Board.ParseBoard("wa1;bb2;bc3;wa7")
            });
            boards.Add(TestBoardTypes.MultiStep, new List<Board>{
                Board.ParseBoard("we3;wg3;bf4;bf6") });
            boards.Add(TestBoardTypes.Void, new List<Board>{
                Board.ParseBoard("wb8;wd8;wf8;wh8;ba1;bc1;be1")
            });

            expectedMoves = new List<IntegrationTestResult>();
            IntegrationTestResult result = new IntegrationTestResult()
            {
                sourseBoardNumber = 1,
                TestType = TestBoardTypes.Simple,
                NextMove = PieceColor.White,
                moves = new List<String>() { "B4 - D2" }
            };
            expectedMoves.Add(result);

            result = new IntegrationTestResult()
            {
                sourseBoardNumber = 1,
                TestType = TestBoardTypes.Simple,
                NextMove = PieceColor.Black,
                moves = new List<String>() { "C3 - A5" }
            };
            expectedMoves.Add(result);

            result = new IntegrationTestResult()
            {
                sourseBoardNumber = 2,
                TestType = TestBoardTypes.Simple,
                NextMove = PieceColor.White,
                moves = new List<String>() { "A7 - B8" }
            };
            expectedMoves.Add(result);

            result = new IntegrationTestResult()
            {
                sourseBoardNumber = 2,
                TestType = TestBoardTypes.Simple,
                NextMove = PieceColor.Black,
                moves = new List<String>() { "B2 - C1", "C3 - D2" }
            };
            expectedMoves.Add(result);

            result = new IntegrationTestResult()
            {
                sourseBoardNumber = 1,
                TestType = TestBoardTypes.Void,
                NextMove = PieceColor.White,
                moves = new List<String>()
            };
            expectedMoves.Add(result);
            result = new IntegrationTestResult()
            {
                sourseBoardNumber = 1,
                TestType = TestBoardTypes.Void,
                NextMove = PieceColor.Black,
                moves = new List<String>()
            };
            expectedMoves.Add(result);
            
            result = new IntegrationTestResult()
            {
                sourseBoardNumber = 1,
                TestType = TestBoardTypes.MultiStep,
                NextMove = PieceColor.White,
                moves = new List<String>() { "E3 - G5 - E7", "G3 - E5 - G7" }
            };
            expectedMoves.Add(result);

            result = new IntegrationTestResult()
            {
                sourseBoardNumber = 1,
                TestType = TestBoardTypes.MultiStep,
                NextMove = PieceColor.Black,
                moves = new List<String>() { "F4 - D2", "F4 - H2"}
            };
            expectedMoves.Add(result);
        }

        private List<Board> GetTestBoards(TestBoardTypes testType)
        {
             return boards[testType];
        }

        private List<IntegrationTestResult> GetResults(TestBoardTypes testType, int boardNo)
        {
            return expectedMoves.Where<IntegrationTestResult>(r => r.TestType == testType && r.sourseBoardNumber == boardNo).ToList();
        }

        private void AssertByType(TestBoardTypes testType)
        {
            int boardCount = 1;
            foreach (Board b in GetTestBoards(testType))
            {
                foreach (IntegrationTestResult res in GetResults(testType, boardCount))
                {
                    List<String> expected = res.moves;
                    IEnumerable<Move> actual = Piece.GetAllMoves(b, res.NextMove);
                    Assert.AreEqual<int>(expected.Count(), actual.Count(), "count of moves");
                    int moveCount = 0;
                    foreach (Move move in actual)
                    {
                        Assert.AreEqual<String>(expected[moveCount], move.ToString());
                        moveCount++;
                    }
                }
                boardCount++;
            }
        }

        [TestMethod]
        public void Board_WithSimpleMoves_ReturnMoves()
        {
            AssertByType(TestBoardTypes.Simple);
        }

        [TestMethod]
        public void Board_WithMultiStepMoves_ReturnMultipleStepMoves()
        {
            AssertByType(TestBoardTypes.MultiStep);
        }

        [TestMethod]
        public void Board_WithNoPossibleMoves_ReturnsNoMoves()
        {
            AssertByType(TestBoardTypes.Void);
        }
    }
}
