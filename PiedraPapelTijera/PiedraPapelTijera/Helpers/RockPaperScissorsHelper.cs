using System;
using System.Collections.Generic;
using PiedraPapelTijera.Constants;
using System.Text;

namespace PiedraPapelTijera.Helpers
{
    public class RockPaperScissorsHelper
    {
        private Dictionary<int, string> rockPaperScissorsDict = new Dictionary<int, string> {
            { 1,GameConstants.Rock},
            { 2,GameConstants.Paper},
            { 3,GameConstants.Scissors}
        };
        public string GenerateItem()
        {
            Random r = new Random();
            int itemId = r.Next(1, 3);
            return rockPaperScissorsDict[itemId];
        }
        public int CalculateResult(string userItem, string pcItem)
        {
            switch(userItem)
            {
                case GameConstants.Rock:
                    if (pcItem == GameConstants.Paper)
                        return -1;
                    else if (pcItem == GameConstants.Rock)
                        return 0;
                    else return 1;
                case GameConstants.Paper:
                    if (pcItem == GameConstants.Scissors)
                        return -1;
                    else if (pcItem == GameConstants.Paper)
                        return 0;
                    else return 1;
                case GameConstants.Scissors:
                    if (pcItem == GameConstants.Rock)
                        return -1;
                    else if (pcItem == GameConstants.Scissors)
                        return 0;
                    else return 1;
                default:
                    return -1;
            }
        }
    }
}
