using System;
using System.Collections.Generic;
using System.Text;

namespace PiedraPapelTijera.Constants
{
    public static class GameConstants
    {
        public const string Rock = "Rock";
        public const string Scissors = "Scissors";
        public const string Paper = "Paper";
        public static Dictionary<int, string> GameResult = new Dictionary<int,string> {
            {1,"YOU WIN"},
            {0,"ITS A TIE"},
            {-1,"YOU LOOSE"},

        };
        public static Dictionary<int, string> AvailablePicksToReceive = new Dictionary<int, string>
        {
            {1, Rock},
            {2, Scissors },
            {3, Paper }
        };
        public static Dictionary<string, int> AvailablePicksToSend = new Dictionary<string, int>
        {
            {"Rock", 1 },
            {"Scissors", 2 },
            {"Paper", 3 }
        };
    }
}
