using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

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
        public static void AddMessageListener(Action<string,string> receiveMessageAction)
        {
            if(mainConnection!=null)
                mainConnection.On<string, string>("ReceiveMessage", (user, message) => receiveMessageAction(user, message));
        }
        public static void AddGameRequestListener(Action<string,int> receiveGameRequestAction)
        {
            if (mainConnection != null)
                mainConnection.On<string, int>("ReceiveGameRequest", (user, nRounds) => receiveGameRequestAction(user, nRounds));
        }
        public static void AddGameTurnListener(Action<int> receiveGameTurn)
        {
            if (mainConnection != null)
                mainConnection.On<int>("ReceiveRivalsPick", (userTurn) => receiveGameTurn(userTurn));
        }
        public static async Task StartConnection(string userName)
        {
            if (mainConnection != null)
                return;
            mainConnection = new HubConnectionBuilder().WithUrl(Constants.ChatConstants.serverUrl).Build();
            await mainConnection.StartAsync();
            await mainConnection.InvokeAsync("NewConnectionAdded", userName);
           
            Console.WriteLine(mainConnection.State);
        }
        public static async void SendMyPick(int myPick, string nextUsername)
        {
            await mainConnection.InvokeAsync("SendUsersPick", myPick, nextUsername);
        }

        public static async void Disconnect(string userName)
        {
            await mainConnection.InvokeAsync("DisconnectFromServer", userName);
        }
        
    }
}
