using PiedraPapelTijera.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.SignalR.Client;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

namespace PiedraPapelTijera
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainMenuPage : ContentPage
    {
        public List<CustomMenuItem> Items { get; set; }

        public MainMenuPage()
        {
            InitializeComponent();
            LoginToGame();
            var goToDetailCommand = new Command(() => { Navigation.PushAsync(new ViewContactsPage()); });
            Items = new List<CustomMenuItem>
            {
                new CustomMenuItem{ Title = "View Contacts", TapCommand= goToDetailCommand},
                new CustomMenuItem{ Title= "Play Game", TapCommand = new Command(() => { Navigation.PushAsync(new MainPage()); })},
                new CustomMenuItem {Title= "Show me my number!", TapCommand = new Command(()=>{ string countryCode = GetPhoneCountryCode(); DisplayAlert("Your number","Your number is... "+countryCode,"OK"); Globals.PhoneCountryCode.PhoneGlobal = countryCode; }) },
                new CustomMenuItem{ Title = "Register", TapCommand= new Command(()=>SetupPopupRegistration())},
            };

            MyListView.ItemsSource = Items;
        }
        private async void LoginToGame()
        {
            await Services.ChatClient.StartConnection(Constants.AppConstants.appUserName);
            Services.ChatClient.AddGameRequestListener(ShowChallengedScreen);
        }
        private  void ShowChallengedScreen(string challengedBy, int noRounds)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                ShowChallengedScreenMainThread(challengedBy,noRounds);
               
            });
           
            /*string message = accept ? "accepted" : "rejected";
            await DisplayAlert("Acceptance", "You just " + message + " the duel", "OK");*/
        }
        private async void ShowChallengedScreenMainThread(string challengedBy, int noRounds)
        {
            bool accepted = await DisplayAlert("Challenged!", "You were challenged by: " + challengedBy + " for " + noRounds + " rounds.", "Accept", "Cancel");
            if (accepted) await Navigation.PushAsync(new MainPage(challengedBy,noRounds));
        }
        public void SetupPopupRegistration()
        {
            MainPageLayout.BackgroundColor = Color.LightGray;
            MyListView.IsVisible = false;
            PopupRegister.IsVisible = true;
            PopupRegister.IsEnabled = true;
            CountryNumber.Text = GetPhoneCountryCode();
        }

        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            await DisplayAlert("Item Tapped", "An item was tapped.", "OK");
            var item = e.Item as CustomMenuItem;
            item.TapCommand.Execute(null);
            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }
        private string GetPhoneCountryCode()
        {
            var deviceInfo = Xamarin.Forms.DependencyService.Get<Interfaces.IDeviceInfo>();
            return deviceInfo.GetPhoneNumber();
        }

        private void DisappearPopup()
        {
            MainPageLayout.BackgroundColor = Color.Default;
            PopupRegister.IsEnabled = false;
            PopupRegister.IsVisible = false;
            MyListView.IsVisible = true;
        }

        private void ButtonOk_Clicked(object sender, EventArgs e)
        {
            DisappearPopup();
            Constants.AppConstants.appUserName = CountryNumber.Text+PhoneEntry.Text;
            DisplayAlert("Registration", "You entered username: "+ Constants.AppConstants.appUserName, "OK");
        }

        private void ButtonCancel_Clicked(object sender, EventArgs e)
        {
            DisappearPopup();
        }

    }
}
