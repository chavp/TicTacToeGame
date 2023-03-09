using TicTacToeGame.Models;

namespace TicTacToeGame.Web.ViewModels
{
    public class TicTacToeBoardVm
    {
        private int _playerOne = 1;
        private int _playerTwo = 4;
        private int _currentPlayer;
        private int _winner;

        private TicTacToeBoard _ticTacToeBoard;
        //private List<ActData> _possibleActs;
        private List<GameSummary> _gameSummaries;
        private GameStat _gameStat;

        public TicTacToeBoardVm()
        {
            init();
        }

        private List<Square> _squareList;

        Random _ran = new Random(DateTime.Now.Second);
        private void init()
        {
            _ticTacToeBoard = new();
            _gameSummaries = new();
            _gameStat = new();

            initGameStates();

        }

        private void initGameStates()
        {
            CurrentPlayer = _playerOne;
            WinnerPlayer = 0;
            _squareList = new List<Square>
            {
                new(0,0), new(0,1), new(0,2),
                new(1,0), new(1,1), new(1,2),
                new(2,0), new(2,1), new(2,2),
            };
        }

        public Task<int> GetCurrentPlayerAsync()
        {
            return Task.FromResult(CurrentPlayer);
        }

        public Task<List<Square>> GetSquaresAsync()
        {
            return Task.FromResult(_squareList);
        }
        public List<Square> Squares
        {
            get
            {
                return _squareList;
            }
        }
        public int CurrentPlayer
        {
            set
            {
                _currentPlayer = value;
            }
            get
            {
                return _currentPlayer;
            }
        }
        public int WinnerPlayer
        {
            set
            {
                _winner = value;
            }
            get
            {
                return _winner;
            }
        }

        public int TotalPossibleActions
        {
            get
            {
                return _ticTacToeBoard.GetPosibleActions().Count;
            }
        }
        public bool Done
        {
            get
            {
                return _ticTacToeBoard.Done;
            }
        }
        public bool CanPlay
        {
            get
            {
                return !_ticTacToeBoard.Done && (TotalPossibleActions > 0);
            }
        }

        public GameStat GameStat
        {
            get
            {
                return _gameStat;
            }
        }

        public Task<(bool, int, int)> GetNextAsync(Square selected)
        {
            selected.Value = CurrentPlayer;
            selected.Disabled = "disabled";


            _ticTacToeBoard.Next(new Act(selected.Row, selected.Col, CurrentPlayer));
            if (_ticTacToeBoard.Done)
            {
                _gameSummaries.Add(_ticTacToeBoard.GameSummary);
                _gameStat = GameStatistics.Stat(_gameSummaries);

                stopGame();
            }
            else if(TotalPossibleActions == 0)
            {
                _gameSummaries.Add(_ticTacToeBoard.GameSummary);
                _gameStat = GameStatistics.Stat(_gameSummaries);
            }
            return Task.FromResult((_ticTacToeBoard.Done, CurrentPlayer, TotalPossibleActions));
        }


        public Task<(bool, int, int)> GetNextAsync()
        {
            return Task.FromResult(Next());
        }

        public (bool, int, int) Next()
        {
            var (done, states, acts, ranAct) = _ticTacToeBoard.Next(CurrentPlayer);
            var sq = _squareList.Single(x => x.Row == ranAct.Row && x.Col == ranAct.Col);
            sq.Value = ranAct.Value;
            sq.Disabled = "disabled";

            if (_ticTacToeBoard.Done)
            {
                _gameSummaries.Add(_ticTacToeBoard.GameSummary);
                _gameStat = GameStatistics.Stat(_gameSummaries);

                stopGame();
            }
            else if (TotalPossibleActions == 0)
            {
                _gameSummaries.Add(_ticTacToeBoard.GameSummary);
                _gameStat = GameStatistics.Stat(_gameSummaries);
            }
            return (_ticTacToeBoard.Done, CurrentPlayer, TotalPossibleActions);
        }

        public Task ResetAsync()
        {

            initGameStates();

            _ticTacToeBoard.Reset();

            return Task.FromResult(0);
        }

        public void TogglePlayer()
        {
            CurrentPlayer = (CurrentPlayer == _playerOne) ? _playerTwo : _playerOne;
        }

        private void stopGame()
        {
            foreach (var square in _squareList)
            {
                square.Disabled = "disabled";
            }

        }
    }

    public class Square
    {
        public Square(int row, int col)
        {
            Row = row;
            Col = col;
        }
        public int Row { get; protected set; }
        public int Col { get; protected set; }
        public int Value { get; set; } = 0;

        public string Disabled { get; set; }

        public string Color
        {
            get
            {
                if (Value == 1)
                    return "bg-success";
                if (Value == 4)
                    return "bg-danger";
                return "";
            }
        }
    }
}
