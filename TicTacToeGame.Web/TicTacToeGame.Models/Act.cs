using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToeGame.Models
{
    public class Act
    {
        public Act(int row, int col, int value)
        {
            Row = row;
            Col = col;
            Value = value;
        }

        public int Value { get; set; }
        public int Row { get; set; }
        public int Col { get; set; }
        public string Name { get; set; } = "PLAY";
        public string ActName
        {
            get 
            { 
                return $"{Value}-{Name}"; 
            }
        }
        public DateTime ActionDateTime { get; set; } = DateTime.Now;
    }
}
