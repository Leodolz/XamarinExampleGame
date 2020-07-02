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

        //Senders
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
       
        public static async Task StartConnection(string userName)
        {
            
            mainConnection = new HubConnectionBuilder().WithUrl(Constants.ChatConstants.serverUrl).Build();
            await mainConnection.StartAsync();
            await mainConnection.InvokeAsync("NewConnectionAdded", userName);
           
            Console.WriteLine(mainConnection.State);
        }
        public static async void SendMyPick(int myPick, string nextUsername)
        {
            await mainConnection.InvokeAsync("SendUsersPick", myPick, nextUsername);
        }
        public static async void LeaveGame(string rivalUsername)
        {
            await mainConnection.InvokeAsync("LeaveGame", rivalUsername);
        }

        public static async void Disconnect(string userName)
        {
            await mainConnection.InvokeAsync("DisconnectFromServer", userName);
        }
        public static async void SendConnectedRequest(string[] contacts)
        {
            await mainConnection.InvokeAsync("SendConnectedToFriends", contacts, Constants.AppConstants.appUserName);
        }
        public static async void SendAcceptGameRequest(int nRounds, string challengedUser)
        {
            await mainConnection.InvokeAsync("AcceptGameRequest", nRounds, challengedUser, Constants.AppConstants.appUserName);
        }
        public static async Task<int> ConsultSingleContactState(string userName)
        {
            return await mainConnection.InvokeAsync<int>("ConsultSingleContactState", userName);
        }
        public static async Task<int[]> ConsultAllContactsState(string[] contacts)
        {
            return await mainConnection.InvokeAsync<int[]>("ConsultContactsStates",contacts);
        }
        public static async void SendChallengeUser(int nRounds, string challengedUser, string ownUsername)
        {
            await mainConnection.InvokeAsync("PlayWithUserRequest", nRounds, challengedUser, ownUsername);
        }


        //Listeners
        public static void AddMessageListener(Action<string, string> receiveMessageAction)
        {
            if (mainConnection != null)
                mainConnection.On<string, string>("ReceiveMessage", (user, message) => receiveMessageAction(user, message));
        }
        public static void AddGameRequestListener(Action<string, int> receiveGameRequestAction)
        {
            if (mainConnection != null)
                mainConnection.On<string, int>("ReceiveGameRequest", (user, nRounds) => receiveGameRequestAction(user, nRounds));
        }
        public static void AddGameTurnListener(Action<int> receiveGameTurn)
        {
            if (mainConnection != null)
                mainConnection.On<int>("ReceiveRivalsPick", (userTurn) => receiveGameTurn(userTurn));
        }
        public static void AddGameCodeListener(Action<int,int,string> receiveGameCode)
        {
            if (mainConnection != null)
                mainConnection.On<int, int,string>("ReceiveGameCode", (response,rounds, against)=> receiveGameCode(response,rounds,against));
        }
        public static void AddGameDisconnectListener(Action receiveDisconnectNotification)
        {
            if (mainConnection != null)
                mainConnection.On("ReceiveDisconnect", () => receiveDisconnectNotification());
        }
        public static void AddFriendConnectedListener(Action<string> receiveConnectedNotification)
        {
            if (mainConnection != null)
                mainConnection.On<string>("ReceiveFriendConnect", (friendUsername) => receiveConnectedNotification(friendUsername));
        }
        public static void AddFriendDisconnectListener(Action<string> receiveDisconnectedNotification)
        {
            if (mainConnection != null)
                mainConnection.On<string>("ReceiveFriendDisconnect", (friendUsername) => receiveDisconnectedNotification(friendUsername));
        }
        
    }
}
