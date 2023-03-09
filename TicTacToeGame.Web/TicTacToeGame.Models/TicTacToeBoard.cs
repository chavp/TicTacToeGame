namespace TicTacToeGame.Models
{
    public class TicTacToeBoard
    {
        public Guid Id { get; protected set; }

        Random _random = new Random(DateTime.Now.Millisecond);
        List<List<int>> _states;
        List<Act> _actEvents;
        private DateTime _gameStart;
        private DateTime? _gameEnd;
        private bool _done;
        public List<List<int>> States
        {
            get { return _states; }
            set { _states = value; }
        }
        public bool Done
        {
            get { return _done; }
        }

        public TicTacToeBoard()
        {
            Reset();
        }

        public (bool, List<List<int>>, List<Act>, Act) Next(int player)
        {
            var possibleActions = GetPosibleActions();
            var actIndex = _random.Next(0, possibleActions.Count);
            var randomAct = possibleActions[actIndex];
            randomAct.Value = player;

            var (done, states, _possibleActs) = Next(randomAct);

            return (done, states, _possibleActs, randomAct);
        }
        public List<Act> GetPosibleActions()
        {
            var posibleActions = new List<Act>();
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (_states[i][j] == 0)
                    {
                        posibleActions.Add(new Act(i, j, 0));
                    }
                }
            }
            return posibleActions;
        }
        private List<int> initRow()
        {
            return new List<int> { 0, 0, 0 };
        }

        public (bool, List<List<int>>, List<Act>) Next(Act act)
        {
            var rowData = _states[act.Row];
            rowData[act.Col] = act.Value;
            _actEvents.Add(act);
            var posibleActions = GetPosibleActions();
            var (done, states, possibleActions) = (isDone(_states), _states, posibleActions);
            _done = done;
            if (done)
            {
                act.Name = "WIN";
            }
            else if (posibleActions.Count == 0)
            {
                act.Name = "DRAW";
            }
            return (done, states, possibleActions);
        }

        private bool isDone(List<List<int>> states)
        {
            var scoreRows = new List<int> { 0, 0, 0 };
            var scoreCols = new List<int> { 0, 0, 0 };
            var scoreDiagonalR = 0;
            var scoreDiagonalL = 0;
            for (int i = 0; i < 3; i++)
            {
                scoreRows[i] = states[i].Sum();
                for (int j = 0; j < 3; j++)
                {
                    scoreCols[i] += states[j][i];
                    if (i == j)
                    {
                        scoreDiagonalR += states[i][j];
                    }
                    if (Math.Abs(i - j) == 2 || i * j == 1)
                    {
                        scoreDiagonalL += states[i][j];
                    }
                }
            }

            for (int i = 0; i < 3; i++)
            {
                // check rows
                if (scoreRows[i] == 3 || scoreRows[i] == 12)
                {
                    _gameEnd = DateTime.Now;
                    return true;
                }

                // check cols
                if (scoreCols[i] == 3 || scoreCols[i] == 12)
                {
                    _gameEnd = DateTime.Now;
                    return true;
                }

                // check diagonal
                if (scoreDiagonalR == 3 || scoreDiagonalR == 12)
                {
                    _gameEnd = DateTime.Now;
                    return true;
                }

                if (scoreDiagonalL == 3 || scoreDiagonalL == 12)
                {
                    _gameEnd = DateTime.Now;
                    return true;
                }
            }

            return false;
        }

        public void ConsolePrint()
        {
            for (int i = 0; i < 3; i++)
            {
                Console.WriteLine($"{string.Join(" ", _states[i])}");
            }
            Console.WriteLine($"---");
            Console.WriteLine($"done = {isDone(_states)}");
        }

        public (bool, List<List<int>>, List<Act>) Reset()
        {
            Id = Guid.NewGuid();
            _states = new();
            _actEvents = new();
            _gameStart = DateTime.Now;
            _gameEnd = null;
            _done = false;

            for (int i = 0; i < 3; i++)
            {
                _states.Add(initRow());
            }

            return (false, _states, GetPosibleActions());
        }

        public void ConsolePrintEvents()
        {
            foreach (var act in _actEvents)
            {
                Console.WriteLine(
                    $"{act.ActionDateTime.ToString("yyyy-MM-dd HH:mm:ss")}, {act.ActName}, {(act.ActionDateTime - _gameStart).TotalSeconds}s");
            }
        }

        public void ConsolePrintGameSummary()
        {
            var gameSummary = GameSummary;
            Console.WriteLine(
                   $"{gameSummary.DateTime.ToString("yyyy-MM-dd HH:mm:ss")}, {gameSummary.Id}, {gameSummary.Status}, {gameSummary.Duration.TotalSeconds}s");
        }

        public GameSummary GameSummary
        {
            get
            {
                var summary = new GameSummary
                {
                    DateTime = _gameStart,
                    Id = Id,
                };
                if(_gameEnd.HasValue)
                {
                    summary.Duration = _gameEnd.Value - _gameStart;
                }
                if( _done && GetPosibleActions().Count >= 0)
                {
                    summary.Status = "";
                    var latestAct = _actEvents.OrderByDescending(x => x.ActionDateTime).First();
                    if(latestAct.Value == 1)
                    {
                        summary.Status = "XWIN";
                    }
                    else
                    {
                        summary.Status = "OWIN";
                    }
                }
                else if(GetPosibleActions().Count == 0)
                {
                    summary.Status = "DRAW";
                }
                return summary;
            }
        }
    }
}