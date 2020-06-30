using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PiedraPapelTijera.Constants;
using PiedraPapelTijera.Helpers;
using PiedraPapelTijera.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace PiedraPapelTijera
{
    public partial class MainPage : ContentPage
    {
        private string selection = "";
        private int totalScore = 0;
        private int rivalsPick = 0;
        private string againstPlayer = null;
        private bool[] matchEnded = new bool[2];
        private int numberRounds = 0;
        private CancellationTokenSource cancellation;
        RockPaperScissorsHelper rockPaperScissorsHelper = new RockPaperScissorsHelper();
        
        //Constructors
        public MainPage()
        {
            InitializeComponent();
            AgainstLabel.Text = "Computer";
            ClearScore.IsVisible = true;
        }
        public MainPage(string againstPlayer, int noRounds)
        {
            InitializeComponent();
            this.againstPlayer = againstPlayer;
            AgainstLabel.Text = againstPlayer;
            cancellation = new CancellationTokenSource();
            Services.ChatClient.AddGameTurnListener(RivalSelected);
            numberRounds = noRounds;
            SetTimer();
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
                        PlayersSelection.SelectedItem = selection;
                        PlayAgainstPlayer();
                    }
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
                    SetTimer();
                    return false;
                }
                else
                {
                    return true;
                }
            });
        }
        //Receiving Routine

        private void RivalSelected (int rivalsPick)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                matchEnded[1] = true;
                this.rivalsPick = rivalsPick;
                CheckIfEnded();
            });
        }

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
            PlayButton.IsEnabled = !matchEnd;
            cpuResultLayout.IsVisible = matchEnd;
            AwaitStack.IsVisible = !matchEnd;
            NextMatchLayout.IsVisible = matchEnd;
            if (matchEnd) StartNextMatchTimer();
        }

        private void Picker_SelectedIndexChanged(object sender, EventArgs e)
        {
            Picker picker = (Picker)sender;
            if (picker.SelectedIndex >= 0)
                selection = picker.Items[picker.SelectedIndex];
        }
        private void SetLayoutVisibles(bool calculateResult)
        {
            PlayButton.IsEnabled = !calculateResult;
            PlayAgainButton.IsEnabled = calculateResult;
            cpuResultLayout.IsVisible = calculateResult;
        }
        private void ClearGame()
        {
            matchEnded[0] = false;
            matchEnded[1] = false;
            rivalsPick = 0;
        }
        private void CalculateResult(string rivalsPick)
        {
            ClearGame();
            ComputerSelectionLabel.Text = rivalsPick;
            int result = rockPaperScissorsHelper.CalculateResult(selection, rivalsPick);
            FinalResultLabel.Text = GameConstants.GameResult[result];
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
            PlayButton.IsEnabled = false;
            Services.ChatClient.SendMyPick(GameConstants.AvailablePicksToSend[selection], againstPlayer);
            AwaitStack.IsVisible = true;
            cancellation.Cancel();
            CheckIfEnded();
        }

        

        private void PlayClicked(object sender, EventArgs e)
        {
            if (againstPlayer == null)
                PlayAgainstPC();
            else
                PlayAgainstPlayer();
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
        /*
        private void MessagesClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ChatPage("Server","1234567"));
         
        }*/
    }
}
