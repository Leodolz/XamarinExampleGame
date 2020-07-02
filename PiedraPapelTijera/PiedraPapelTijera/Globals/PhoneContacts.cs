using PiedraPapelTijera.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PiedraPapelTijera.Globals
{
    public static class PhoneContacts
    {
        public static List<CustomContact> AllCustomContacts { get; set; } = new List<CustomContact>();
        public static Dictionary<string, string> PhoneToNameDict { get; set; } = new Dictionary<string, string>();
    }
}
