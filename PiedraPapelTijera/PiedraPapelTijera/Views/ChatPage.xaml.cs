using PiedraPapelTijera.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PiedraPapelTijera.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ChatPage : ContentPage
	{
        private static ChatPageViewModel chatContext;
        private static string userNamePhone = "+59179783096";

        public ChatPage (string receiverName, string receiverPhone)
		{
			InitializeComponent ();
            chatContext = new ChatPageViewModel(receiverName,receiverPhone, userNamePhone);
            Services.ChatClient.StartConnection(userNamePhone, OnReceiveMessage);
            BindingContext = chatContext;
            chatContext.Messages.Insert(0, new Models.Message { Text = "This is a test from constructor", Sender = "Me" });
        }
        private static void OnReceiveMessage(string user, string message)
        {
            chatContext.Messages.Insert(0,new Models.Message { Text=message, Sender=user});
        }

        protected override bool OnBackButtonPressed()
        {
            base.OnBackButtonPressed();
            Services.ChatClient.Disconnect(userNamePhone);
            return false;
        }
        public void ScrollTap(object sender, EventArgs e)
        {
            lock (new object())
            {
                if (BindingContext != null)
                {
                    var vm = BindingContext as ChatPageViewModel;

                    Device.BeginInvokeOnMainThread(() =>
                    {
                        while (vm.DelayedMessages.Count > 0)
                        {
                            vm.Messages.Insert(0, vm.DelayedMessages.Dequeue());
                        }
                        vm.ShowScrollTap = false;
                        vm.LastMessageVisible = true;
                        vm.PendingMessageCount = 0;
                        ChatList?.ScrollToFirst();
                        
                    });


                }

            }
        }

        public void OnListTapped(object sender, ItemTappedEventArgs e)
        {
            ChatEntry.UnFocusEntry();
        }
    }
   
}