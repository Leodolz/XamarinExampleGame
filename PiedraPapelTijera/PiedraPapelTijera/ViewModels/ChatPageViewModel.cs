using PiedraPapelTijera.Constants;
using PiedraPapelTijera.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace PiedraPapelTijera.ViewModels
{
    class ChatPageViewModel : INotifyPropertyChanged
    {
        public bool ShowScrollTap { get; set; }
        public bool LastMessageVisible { get; set; } = true;
        public int PendingMessageCount { get; set; } = 0;
        public bool PendingMessageCountVisible { get { return PendingMessageCount > 0; } }
        public string ReceiverName { get; set; }
        public string ReceiverPhone { get; set; }
        public Queue<Message> DelayedMessages { get; set; } = new Queue<Message>();
        public ObservableCollection<Message> Messages { get; set; } = new ObservableCollection<Message>();
        public string TextToSend { get; set; }
        public ICommand OnSendCommand { get; set; }
        public ICommand MessageAppearingCommand { get; set; }
        public ICommand MessageDisappearingCommand { get; set; }

        public ChatPageViewModel(string receiverName, string receiverPhone, string userNamePhone)
        {
            Messages.Insert(0, new Message() { Text = "Hi" });
            Messages.Insert(0, new Message() { Text = "How are you?", Reciever = ChatConstants.defaultReciever });
            Messages.Insert(0, new Message() { Text = "What's new?" });
            Messages.Insert(0, new Message() { Text = "How is your family", Reciever = ChatConstants.defaultReciever });
            Messages.Insert(0, new Message() { Text = "How is your dog?", Reciever = ChatConstants.defaultReciever });
            Messages.Insert(0, new Message() { Text = "How is your cat?", Reciever = ChatConstants.defaultReciever });
            Messages.Insert(0, new Message() { Text = "How is your sister?" });
            Messages.Insert(0, new Message() { Text = "When we are going to meet?" });
            Messages.Insert(0, new Message() { Text = "I want to buy a laptop" });
            Messages.Insert(0, new Message() { Text = "Where I can find a good one?" });
            Messages.Insert(0, new Message() { Text = "Also I'm testing this chat" });
            Messages.Insert(0, new Message() { Text = "Oh My God!" });
            Messages.Insert(0, new Message() { Text = " No Problem", Reciever = ChatConstants.defaultReciever });
            Messages.Insert(0, new Message() { Text = "Hugs and Kisses", Reciever = ChatConstants.defaultReciever });
            Messages.Insert(0, new Message() { Text = "When we are going to meet?" });
            Messages.Insert(0, new Message() { Text = "I want to buy a laptop" });
            Messages.Insert(0, new Message() { Text = "Where I can find a good one?" });
            Messages.Insert(0, new Message() { Text = "Also I'm testing this chat" });
            Messages.Insert(0, new Message() { Text = "Oh My God!" });
            Messages.Insert(0, new Message() { Text = " No Problem" });
            Messages.Insert(0, new Message() { Text = "Hugs and Kisses" });
            ReceiverPhone = receiverPhone.Replace(" ","").Trim();
            ReceiverName = receiverName;
            MessageAppearingCommand = new Command<Message>(OnMessageAppearing);
            MessageDisappearingCommand = new Command<Message>(OnMessageDisappearing);
            ShowScrollTap = true;
            OnSendCommand = new Command(() =>
            {
                if (!string.IsNullOrEmpty(TextToSend))
                {
                    Messages.Insert(0, new Message() { Text = TextToSend, Reciever = ChatConstants.defaultReciever });
                    Services.ChatClient.SendPersonalMessage(userNamePhone, TextToSend,ReceiverPhone);
                    Console.WriteLine("------------------------------------------------------");
                    Console.WriteLine("Sent message to: " + ReceiverPhone);
                    Console.WriteLine("------------------------------------------------------");
                    TextToSend = string.Empty;
                }

            });

            //Code to simulate reveing a new message procces
            /*
            Device.StartTimer(TimeSpan.FromSeconds(5), () =>
            {
                if (LastMessageVisible)
                {
                    Messages.Insert(0, new Message() { Text = "New message test", Reciever = "Mario" });
                }
                else
                {
                    DelayedMessages.Enqueue(new Message() { Text = "New message test", Reciever = "Mario" });
                    PendingMessageCount++;
                }
                return true;
            });*/



        }

        void OnMessageAppearing(Message message)
        {
            var idx = Messages.IndexOf(message);
            if (idx <= 6)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    while (DelayedMessages.Count > 0)
                    {
                        Messages.Insert(0, DelayedMessages.Dequeue());
                    }
                    ShowScrollTap = false;
                    LastMessageVisible = true;
                    PendingMessageCount = 0;
                });
            }
        }

        void OnMessageDisappearing(Message message)
        {
            var idx = Messages.IndexOf(message);
            if (idx >= 6)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    ShowScrollTap = true;
                    LastMessageVisible = false;
                });

            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
    }
}
