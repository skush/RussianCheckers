using System;
using System.Collections.Generic;
using System.Linq;
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
            public int SourseBoardNumber;
            public TestBoardTypes TestType;
            public PieceColor NextMove;
            public List<String> Moves;
        }

        static Dictionary<TestBoardTypes, List<Board>> _boards;

        static List<IntegrationTestResult> _expectedMoves;

        [ClassInitialize()]
        public static void IntegrationTestInit(TestContext context)
        {
            _boards = new Dictionary<TestBoardTypes, List<Board>>
            {
                {
                    TestBoardTypes.Simple, new List<Board>
                    {
                        Board.ParseBoard("wa1;bb2;bc3;wb4"),
                        Board.ParseBoard("wa1;bb2;bc3;wa7")
                    }
                },
                {
                    TestBoardTypes.MultiStep, new List<Board>
                    {
                        Board.ParseBoard("we3;wg3;bf4;bf6")
                    }
                },
                {
                    TestBoardTypes.Void, new List<Board>
                    {
                        Board.ParseBoard("wb8;wd8;wf8;wh8;ba1;bc1;be1")
                    }
                }
            };

            _expectedMoves = new List<IntegrationTestResult>();
            var result = new IntegrationTestResult()
            {
                SourseBoardNumber = 1,
                TestType = TestBoardTypes.Simple,
                NextMove = PieceColor.White,
                Moves = new List<String>() { "B4 - D2" }
            };
            _expectedMoves.Add(result);

            result = new IntegrationTestResult()
            {
                SourseBoardNumber = 1,
                TestType = TestBoardTypes.Simple,
                NextMove = PieceColor.Black,
                Moves = new List<String>() { "C3 - A5" }
            };
            _expectedMoves.Add(result);

            result = new IntegrationTestResult()
            {
                SourseBoardNumber = 2,
                TestType = TestBoardTypes.Simple,
                NextMove = PieceColor.White,
                Moves = new List<String>() { "A7 - B8" }
            };
            _expectedMoves.Add(result);

            result = new IntegrationTestResult()
            {
                SourseBoardNumber = 2,
                TestType = TestBoardTypes.Simple,
                NextMove = PieceColor.Black,
                Moves = new List<String>() { "B2 - C1", "C3 - D2" }
            };
            _expectedMoves.Add(result);

            result = new IntegrationTestResult()
            {
                SourseBoardNumber = 1,
                TestType = TestBoardTypes.Void,
                NextMove = PieceColor.White,
                Moves = new List<String>()
            };
            _expectedMoves.Add(result);
            result = new IntegrationTestResult()
            {
                SourseBoardNumber = 1,
                TestType = TestBoardTypes.Void,
                NextMove = PieceColor.Black,
                Moves = new List<String>()
            };
            _expectedMoves.Add(result);
            
            result = new IntegrationTestResult()
            {
                SourseBoardNumber = 1,
                TestType = TestBoardTypes.MultiStep,
                NextMove = PieceColor.White,
                Moves = new List<String>() { "E3 - G5 - E7", "G3 - E5 - G7" }
            };
            _expectedMoves.Add(result);

            result = new IntegrationTestResult()
            {
                SourseBoardNumber = 1,
                TestType = TestBoardTypes.MultiStep,
                NextMove = PieceColor.Black,
                Moves = new List<String>() { "F4 - D2", "F4 - H2"}
            };
            _expectedMoves.Add(result);
        }

        private List<Board> GetTestBoards(TestBoardTypes testType)
        {
             return _boards[testType];
        }

        private List<IntegrationTestResult> GetResults(TestBoardTypes testType, int boardNo)
        {
            return _expectedMoves.Where<IntegrationTestResult>(r => r.TestType == testType && r.SourseBoardNumber == boardNo).ToList();
        }

        private void AssertByType(TestBoardTypes testType)
        {
            int boardCount = 1;
            foreach (Board b in GetTestBoards(testType))
            {
                foreach (IntegrationTestResult res in GetResults(testType, boardCount))
                {
                    List<String> expected = res.Moves;
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
