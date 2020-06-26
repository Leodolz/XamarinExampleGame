using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace PiedraPapelTijera.Models
{
    public class CustomContact: Plugin.ContactService.Shared.Contact
    {
        public CustomContact(Plugin.ContactService.Shared.Contact contact, bool playing, ICommand TapCommand)
        {
            Name = contact.Name;
            Number = contact.Number!=null?contact.Number.StartsWith("+")?contact.Number:Globals.PhoneCountryCode.PhoneGlobal+contact.Number:"";
            Playing = playing;
            this.TapCommand = TapCommand;
        }
        public bool Playing { get; set; }
        public ICommand TapCommand { get; set; } 
    }
}
