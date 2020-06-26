using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PiedraPapelTijera.Constants;
using PiedraPapelTijera.Helpers;
using PiedraPapelTijera.Views;
using Xamarin.Forms;

namespace PiedraPapelTijera
{
    public partial class MainPage : ContentPage
    {
        private string selection = "";
        private int totalScore = 0;
        public MainPage()
        {
            InitializeComponent();
            
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

        private void PlayClicked(object sender, EventArgs e)
        {
            SetLayoutVisibles(true);
            RockPaperScissorsHelper rockPaperScissorsHelper = new RockPaperScissorsHelper();
            string cpuPick = rockPaperScissorsHelper.GenerateItem();
            ComputerSelectionLabel.Text = cpuPick;
            int result = rockPaperScissorsHelper.CalculateResult(selection, cpuPick);
            FinalResultLabel.Text = GameConstants.GameResult[result];
            totalScore = result == 1 ? totalScore + 1 : totalScore;
            ComputerScore.Text = totalScore.ToString();
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
        private void MessagesClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ChatPage("Server","1234567"));
         
        }
    }
}
