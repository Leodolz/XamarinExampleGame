﻿using PiedraPapelTijera.ViewModels;
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
        private string receiverPhone = "";
        public ChatPage (string receiverName, string receiverPhone)
		{
			InitializeComponent ();
            this.receiverPhone = receiverPhone;
            chatContext = new ChatPageViewModel(receiverName,receiverPhone, Constants.AppConstants.appUserName);
            Services.ChatClient.AddMessageListener(OnReceiveMessage);
            BindingContext = chatContext;
            
        }
        private void OnReceiveMessage(string user, string message)
        {
            if (receiverPhone.Replace(" ", "").Trim() == user.Replace(" ", "").Trim())
                chatContext.Messages.Insert(0, new Models.Message { Text = message, Sender = user });
            else Console.WriteLine("---------------------Rejected!");
        }
        /*
        protected override bool OnBackButtonPressed()
        {
            base.OnBackButtonPressed();
            Services.ChatClient.Disconnect(userNamePhone);
            return false;
        }*/
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