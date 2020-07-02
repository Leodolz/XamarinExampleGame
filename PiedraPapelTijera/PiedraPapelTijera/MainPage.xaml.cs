using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PiedraPapelTijera.Constants;
using PiedraPapelTijera.Helpers;
using PiedraPapelTijera.Models;
using PiedraPapelTijera.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace PiedraPapelTijera
{
    public partial class MainPage : ContentPage
    {
        private string selection = "";
        public List<CustomGameItem> Items { get; set; }
        private int totalScore = 0;
        private int rivalsPick = 0;
        private string againstPlayer = null;
        private bool[] matchEnded = new bool[2];
        private int roundNo = 1;
        private CancellationTokenSource cancellation;
        RockPaperScissorsHelper rockPaperScissorsHelper = new RockPaperScissorsHelper();

        
        
        //Constructors
        public MainPage()
        {
            InitializeComponent();
            CreateGameItems();
            AgainstLabel.Text = "Computer";
            ClearScore.IsVisible = true;
        }
        public MainPage(string againstPlayer)
        {
            InitializeComponent();
            CreateGameItems();
            this.againstPlayer = againstPlayer;
            AgainstLabel.Text = againstPlayer;
            cancellation = new CancellationTokenSource();
            Services.ChatClient.AddGameTurnListener(RivalSelected);
            Services.ChatClient.AddGameDisconnectListener(RivalDisconnected);
            SetTimer();
        }

        private void CreateGameItems()
        {
            Items = new List<CustomGameItem>
            {
                new CustomGameItem {Title = GameConstants.Paper, ImageSource="paperImage.png"},
                new CustomGameItem {Title = GameConstants.Rock, ImageSource="rockImage.png"},
                new CustomGameItem {Title = GameConstants.Scissors, ImageSource="scissorsImage.png"}
            };
            MyListView.ItemsSource = Items;
            MyListView.HeightRequest = 50;
        }


        //Timers
        private void SetTimer()
        {
            CancellationTokenSource cts = cancellation;
            int gCounter = 5;
            bool cancelled = false;
            Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            {
                if (cts.IsCancellationRequested)
                {
                    cancelled = true;
                }
                gCounter--;

                if (gCounter < 0)
                {
                    if (!cancelled)
                    {
                        selection = rockPaperScissorsHelper.GenerateItem();
                        PlayersPick.Text = selection;
                        MyListView.IsVisible = false;
                        PlayAgainstPlayer();
                    }
                    cancelled = false;
                    return false;
                }
                else
                {
                    AwaitTimer.Text = gCounter.ToString();
                    return true;
                }
                
            });

        }
        private void StartNextMatchTimer()
        {
            int nextMatch = 3;
            Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            {
                NextMatchTimer.Text = nextMatch.ToString();
                nextMatch--;
                if (nextMatch < 0)
                {
                    SetVisiblesMultiPlayer(false);
                    ClearGame();
                    SetTimer();
                    return false;
                }
                else
                {
                    return true;
                }
            });
        }
        //Receiving Routines
        // ------------------------
        private void RivalSelected (int rivalsPick)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                matchEnded[1] = true;
                this.rivalsPick = rivalsPick;
                CheckIfEnded();
            });
        }

        private async void RivalDisconnected()
        {
            MainThread.BeginInvokeOnMainThread(async ()=>
            {
                await DisplayAlert("Disconected", "Your rival desconected from the match", "OK");
                await Navigation.PushAsync( await Navigation.PopAsync());
            });
        }

        //--------------------------------------
        private void CheckIfEnded()
        {
            if (matchEnded[0] & matchEnded[1])
            {
                CalculateResult(GameConstants.AvailablePicksToReceive[rivalsPick]);
                SetVisiblesMultiPlayer(true);
            }
        }

        private void SetVisiblesMultiPlayer(bool matchEnd)
        {
            cpuResultLayout.IsVisible = matchEnd;
            AwaitStack.IsVisible = !matchEnd;
            NextMatchLayout.IsVisible = matchEnd;
            MyListView.IsVisible = !matchEnd;
            if (matchEnd) StartNextMatchTimer();
        }

        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;
            var item = e.Item as CustomGameItem;
            selection = item.Title;
            MyListView.IsVisible = false;
            if (againstPlayer == null)
                PlayAgainstPC();
            else
                PlayAgainstPlayer();
            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }
        private void SetLayoutVisibles(bool calculateResult)
        {
            PlayAgainButton.IsEnabled = calculateResult;
            cpuResultLayout.IsVisible = calculateResult;
            MyListView.IsVisible = !calculateResult;
        }
        private void ClearGame()
        {
            matchEnded[0] = false;
            matchEnded[1] = false;
            rivalsPick = 0;
            PlayersPick.Text = "(Choose One)";
            roundNo++;
            RoundLabel.Text = roundNo.ToString();
            cancellation = new CancellationTokenSource();
        }
        private void CalculateResult(string rivalsPick)
        {
            ComputerSelectionLabel.Text = rivalsPick;
            int result = rockPaperScissorsHelper.CalculateResult(selection, rivalsPick);
            FinalResultLabel.Text = GameConstants.GameResult[result];
            FinalResultLabel.TextColor = GameConstants.GameResultColor[result];
            totalScore = result == 1 ? totalScore + 1 : totalScore;
            ComputerScore.Text = totalScore.ToString();
        }
        private void PlayAgainstPC()
        {
            SetLayoutVisibles(true);
            string cpuPick = rockPaperScissorsHelper.GenerateItem();
            CalculateResult(cpuPick);
            
        }
        private void PlayAgainstPlayer()
        {
            matchEnded[0] = true;
            PlayersPick.Text = selection;
            Services.ChatClient.SendMyPick(GameConstants.AvailablePicksToSend[selection], againstPlayer);
            AwaitStack.IsVisible = true;
            cancellation.Cancel();
            CheckIfEnded();
        }
        protected override bool OnBackButtonPressed()
        {
            base.OnBackButtonPressed();
            Services.ChatClient.Disconnect(AppConstants.appUserName);
            return false;
        }



        private void PlayAgainClicked(object sender, EventArgs e)
        {
            SetLayoutVisibles(false);
        }
        private void ClearScoreClicked(object sender, EventArgs e)
        {
            totalScore = 0;
            SetLayoutVisibles(false);
            ComputerScore.Text = totalScore.ToString();
        }

        private async void ButtonLeave_Clicked(object sender, EventArgs e)
        {
            if(againstPlayer!=null)
                Services.ChatClient.LeaveGame(againstPlayer);
            await Navigation.PushAsync(await Navigation.PopAsync());
        }

        /*
        private void MessagesClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ChatPage("Server","1234567"));
         
        }*/
    }
}
