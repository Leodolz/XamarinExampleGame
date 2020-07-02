using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace PiedraPapelTijera.Models
{
    public class CustomContact: Plugin.ContactService.Shared.Contact
    {
        public CustomContact(Plugin.ContactService.Shared.Contact contact, string playingState, ICommand TapCommand)
        {
            Name = contact.Name;
            if (contact.Number != null)
            {
                if (contact.Number.Contains("+"))
                    Number = contact.Number.Trim().Replace(" ","");
                else Number = (Globals.PhoneCountryCode.PhoneGlobal + contact.Number).Trim().Replace(" ", "");
            }
            else Number = "";
            //Number = contact.Number!=null?contact.Number.StartsWith("+")?contact.Number:Globals.PhoneCountryCode.PhoneGlobal+contact.Number:"";
            PlayingState = playingState;
            this.TapCommand = TapCommand;
        }
        public string PlayingState { get; set; }
        public ICommand TapCommand { get; set; } 
    }
}
