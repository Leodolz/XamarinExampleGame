using PiedraPapelTijera.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PiedraPapelTijera
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainMenuPage : ContentPage
    {
        public List<CustomMenuItem> Items { get; set; }
        private string userName = "";

        public MainMenuPage()
        {
            InitializeComponent();

            var goToDetailCommand = new Command(() => { Navigation.PushAsync(new ViewContactsPage()); });
            Items = new List<CustomMenuItem>
            {
                new CustomMenuItem{ Title = "View Contacts", TapCommand= goToDetailCommand},
                new CustomMenuItem{ Title= "Play Game", TapCommand = new Command(() => { Navigation.PushAsync(new MainPage()); })},
                new CustomMenuItem {Title= "Show me my number!", TapCommand = new Command(()=>{ DisplayAlert("Your number","Your number is... "+GetNumber(),"OK"); }) },
                new CustomMenuItem{ Title = "Register", TapCommand= new Command(()=>SetupPopupRegistration())},
            };

            MyListView.ItemsSource = Items;
        }
        private void SetupPopupRegistration()
        {
            MainPageLayout.BackgroundColor = Color.LightGray;
            MyListView.IsVisible = false;
            PopupRegister.IsVisible = true;
            PopupRegister.IsEnabled = true;
            CountryNumber.Text = GetNumber();
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
        private string GetNumber()
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
            userName = CountryNumber.Text+PhoneEntry.Text;
            DisplayAlert("Registration", "You entered username: "+userName, "OK");
        }

        private void ButtonCancel_Clicked(object sender, EventArgs e)
        {
            DisappearPopup();
        }

    }
}
