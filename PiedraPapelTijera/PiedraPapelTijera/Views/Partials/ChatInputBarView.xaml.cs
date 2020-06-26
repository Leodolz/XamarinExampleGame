using PiedraPapelTijera.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PiedraPapelTijera.Views.Partials
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ChatInputBarView : ContentView
	{
        public ChatInputBarView()
        {
            InitializeComponent();
            if (Device.RuntimePlatform == Device.iOS)
            {
                SetBinding(HeightRequestProperty, new Binding("Height", BindingMode.OneWay, null, null, null, chatTextInput));
            }
        }
        public void Handle_Completed(object sender, EventArgs e)
        {
            (this.Parent.Parent.BindingContext as ChatPageViewModel).OnSendCommand.Execute(null);
            chatTextInput.Focus();
            chatTextInput.Text = "";

        }

        public void UnFocusEntry()
        {
            chatTextInput?.Unfocus();
        }
    }
}