using System;
using System.Collections.Generic;
using System.Text;

namespace PiedraPapelTijera
{
    public static class Constants
    {
        public const string Rock = "Rock";
        public const string Scissors = "Scissors";
        public const string Paper = "Paper";
        public static Dictionary<int, string> GameResult = new Dictionary<int,string> {
            {1,"YOU WIN"},
            {0,"ITS A TIE"},
            {-1,"YOU LOOSE"},

        };
    }
}
