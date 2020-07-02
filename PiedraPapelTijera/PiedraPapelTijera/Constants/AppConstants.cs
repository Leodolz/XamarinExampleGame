using System;
using System.Collections.Generic;
using System.Text;

namespace PiedraPapelTijera.Constants
{
    public static class AppConstants
    {
        public static string appUserName { get; set; } = "+59179783096";
        public static Dictionary<int, string> userStateDict = new Dictionary<int, string>
        {
            {1,"Online"},
            {0, "Playing" },
            {2, "Offline" }
        };
    }
}
