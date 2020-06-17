using System;
using System.Collections.Generic;
using System.Text;

namespace PiedraPapelTijera
{
    public class RockPaperScissorsHelper
    {
        private Dictionary<int, string> rockPaperScissorsDict = new Dictionary<int, string> {
            { 1,Constants.Rock},
            { 2,Constants.Paper},
            { 3,Constants.Scissors}
        };
        public string GenerateItem()
        {
            Random r = new Random();
            int itemId = r.Next(1, 3);
            return rockPaperScissorsDict[itemId];
        }
        public int calculateResult(string userItem, string pcItem)
        {
            switch(userItem)
            {
                case Constants.Rock:
                    if (pcItem == Constants.Paper)
                        return -1;
                    else if (pcItem == Constants.Rock)
                        return 0;
                    else return 1;
                case Constants.Paper:
                    if (pcItem == Constants.Scissors)
                        return -1;
                    else if (pcItem == Constants.Paper)
                        return 0;
                    else return 1;
                case Constants.Scissors:
                    if (pcItem == Constants.Rock)
                        return -1;
                    else if (pcItem == Constants.Scissors)
                        return 0;
                    else return 1;
                default:
                    return -1;
            }
        }
    }
}
