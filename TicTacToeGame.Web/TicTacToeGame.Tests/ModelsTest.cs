using TicTacToeGame.Models;

namespace TicTacToeGame.Tests
{
    [TestClass]
    public class ModelsTest
    {
        int _p1 = 1;
        int _p2 = 4;

        [TestMethod]
        public void TestGames()
        {
            var ticTacToeBoard = new TicTacToeBoard();
            ticTacToeBoard.Reset();
            ticTacToeBoard.Next(new Act(0, 0, _p1));
            ticTacToeBoard.Next(new Act(1, 0, _p1));
            ticTacToeBoard.Next(new Act(2, 0, _p1));
            ticTacToeBoard.ConsolePrint();

            ticTacToeBoard.Reset();
            ticTacToeBoard.Next(new Act(0, 0, _p2));
            ticTacToeBoard.Next(new Act(1, 1, _p2));
            ticTacToeBoard.Next(new Act(2, 2, _p2));
            ticTacToeBoard.ConsolePrint();
        }

        [TestMethod]
        public void TestGameHistory()
        {
            var random = new Random();
            var ticTacToeBoard = new TicTacToeBoard();
            var nextPlayer = _p1;
            var (done, currentStates, possibleActions, ranAct) = ticTacToeBoard.Next(nextPlayer);
            ticTacToeBoard.ConsolePrint();
            while (!done && possibleActions.Count > 0)
            {
                
                if (nextPlayer == _p1)
                {
                    nextPlayer = _p2;
                }
                else
                {
                    nextPlayer = _p1;
                }
                (done, currentStates, possibleActions, ranAct) = ticTacToeBoard.Next(nextPlayer);
                ticTacToeBoard.ConsolePrint();

            }
            Console.WriteLine("print events");
            ticTacToeBoard.ConsolePrintEvents();

        }

        [TestMethod]
        public void TestGameSummary()
        {
            var random = new Random();
            var ticTacToeBoard = randomAiPlay();
            ticTacToeBoard.ConsolePrintGameSummary();

        }

        TicTacToeBoard randomAiPlay()
        {
            var random = new Random();
            var ticTacToeBoard = new TicTacToeBoard();
            var nextPlayer = _p1;
            var (done, currentStates, possibleActions, ranAct) = ticTacToeBoard.Next(nextPlayer);
            //ticTacToeBoard.ConsolePrint();
            while (!done && possibleActions.Count > 0)
            {
                //Task.Delay(TimeSpan.FromMilliseconds(300)).Wait();

                if (nextPlayer == _p1)
                {
                    nextPlayer = _p2;
                }
                else
                {
                    nextPlayer = _p1;
                }
                (done, currentStates, possibleActions, ranAct) = ticTacToeBoard.Next(nextPlayer);
                //ticTacToeBoard.ConsolePrint();

            }
            return ticTacToeBoard;
        }

        [TestMethod]
        public void TestGameStatistics()
        {
            var maxPlay = 1000;
            var gameSummaries = new List<GameSummary>();
            for (int i = 0; i < maxPlay; i++)
            {
                var ticTacToeBoard = randomAiPlay();
                gameSummaries.Add(ticTacToeBoard.GameSummary);
            }

            var gameStat = GameStatistics.Stat(gameSummaries);
            gameStat.ConsolePrint();
        }
    }
}