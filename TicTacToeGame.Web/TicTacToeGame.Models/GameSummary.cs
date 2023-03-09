using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToeGame.Models
{
    public class GameSummary
    {
        public DateTime DateTime { get; set; }
        public Guid Id { get; set; }
        public string Status { get; set; }
        public TimeSpan Duration { get; set; }
    }

    public class GameStat
    {
        public int TotalPlay { get; set; }
        public int XWin { get; set; }
        public int OWin { get; set; }
        public int Draw { get; set; }

        public decimal XWinPercent
        {
            get
            {
                if(TotalPlay == 0) return 0;
                return ((decimal)XWin / TotalPlay) * 100;
            }
        }
        public decimal OWinPercent 
        {
            get
            {
                if (TotalPlay == 0) return 0;
                return ((decimal)OWin / TotalPlay) * 100;
            }
        }
        public decimal DrawPercent 
        {
            get
            {
                if (TotalPlay == 0) return 0;
                return ((decimal)Draw / TotalPlay) * 100;
            }
        }

        public void ConsolePrint()
        {
            Console.WriteLine($"{XWinPercent}%, {OWinPercent}%, {DrawPercent}%");
        }
    }

    public static class GameStatistics
    {
        public static GameStat Stat(List<GameSummary> gameSummaries)
        {
            GameStat gameStat = new();
            if (gameSummaries.Any())
            {
                gameStat.TotalPlay = gameSummaries.Count;
                gameStat.XWin = gameSummaries.Count(x => x.Status == "XWIN");
                gameStat.OWin = gameSummaries.Count(x => x.Status == "OWIN");
                gameStat.Draw = gameSummaries.Count(x => x.Status == "DRAW");
            }
            return gameStat;
        }
    }
}
