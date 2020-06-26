using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace PiedraPapelTijera.Services
{
    public class ChatClient
    {
        private static HubConnection mainConnection = null;

        public static async void SendBroadCastMessage(string user, string message)
        {
            await mainConnection.InvokeAsync("SendBroadcastMessage",
                user, message);
        }
        public static async void SendPersonalMessage(string user, string message, string receiver)
        {
            await mainConnection.InvokeAsync("SendMessage",
                user, message, receiver);
        }
        public static async void StartConnection(string userName, Action<string, string> receiveAction)
        {
            if (mainConnection != null)
                return;
            mainConnection = new HubConnectionBuilder().WithUrl(Constants.ChatConstants.serverUrl).Build();
            mainConnection.On<string, string>("ReceiveMessage", (user, message) => receiveAction(user, message));
            await mainConnection.StartAsync();
            await mainConnection.InvokeAsync("NewConnectionAdded", userName);
            Console.WriteLine(mainConnection.State);
            
        }
        public static async void Disconnect(string userName)
        {
            await mainConnection.InvokeAsync("DisconnectFromServer", userName);
        }
        
    }
}
