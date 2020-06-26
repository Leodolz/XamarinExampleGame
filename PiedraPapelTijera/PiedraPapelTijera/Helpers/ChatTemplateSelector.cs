using PiedraPapelTijera.Models;
using PiedraPapelTijera.Views.Cells;
using PiedraPapelTijera.Constants;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace PiedraPapelTijera.Helpers
{
    class ChatTemplateSelector: DataTemplateSelector
    {
        DataTemplate incomingDataTemplate;
        DataTemplate outgoingDataTemplate;

        public ChatTemplateSelector()
        {
            incomingDataTemplate = new DataTemplate(typeof(IncomingViewCell));
            outgoingDataTemplate = new DataTemplate(typeof(OutgoingViewCell));
        }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var messageVm = item as Message;
            if (messageVm == null)
                return null;


            return (messageVm.Reciever == ChatConstants.defaultReciever) ? outgoingDataTemplate : incomingDataTemplate;
        }
    }
}
