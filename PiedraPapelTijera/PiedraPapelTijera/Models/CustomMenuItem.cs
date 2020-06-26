using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace PiedraPapelTijera.Models
{
    public class CustomMenuItem
    {
        public string Title { get; set; }
        public ICommand TapCommand { get; set; }
    }
}
