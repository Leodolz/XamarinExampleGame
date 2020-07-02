using PiedraPapelTijera.Globals;
using PiedraPapelTijera.Models;
using PiedraPapelTijera.Views;
using Plugin.ContactService.Shared;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PiedraPapelTijera
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainTabbedMenu : TabbedPage
    {
        public List<CustomMenuItem> Items { get; set; }
        bool logedIn = false;
        public MainTabbedMenu ()
        {
            InitializeComponent();
            RequestAllPermissions();
            Globals.PhoneCountryCode.PhoneGlobal = GetPhoneCountryCode();
            ContactsTab.IsVisible = false;
            SetupPopupRegistration();
            Items = new List<CustomMenuItem>
            {
                new CustomMenuItem{ Title= "Play Against PC", TapCommand = new Command(() => {  Console.WriteLine("Tapped!"); Navigation.PushAsync(new MainPage()); })},
            };

            MyListView.ItemsSource = Items;
        }

        private async void RequestAllPermissions()
        {
            
            var status = await CrossPermissions.Current.CheckPermissionStatusAsync<ContactsPermission>();
            if (status != Plugin.Permissions.Abstractions.PermissionStatus.Granted)
            {
                status = await CrossPermissions.Current.RequestPermissionAsync<ContactsPermission>();
            }
            if(status == Plugin.Permissions.Abstractions.PermissionStatus.Granted)
                GetContacts();
            //var requestPermissions = DependencyService.Get<Interfaces.IPermissionUtil>();
            //requestPermissions.requestPermission();
        }

        //CONTACTS METHODS-----------------------------------------------------------------------
        private async void GetContacts()
        {
            HomeListView.BeginRefresh();
            IList<Contact> contacts = await Plugin.ContactService.CrossContactService.Current.GetContactListAsync();
            List<CustomContact> customContacts = new List<CustomContact>();
            Dictionary<string, string> phoneToNameDict = new Dictionary<string, string>();
            foreach(Contact contact in contacts)
            {
                customContacts.Add(new CustomContact(
                    contact,
                    "Offline",
                    new Command(() => {
                        MainThread.BeginInvokeOnMainThread(() => { Navigation.PushAsync(new ChatPage(contact.Name, contact.Number)); });
                        
                    })//Navigation.PushAsync(new ChatPage(contacts[i].Name, contacts[i].Number)))
                    ));
                int customContactNo = contacts.IndexOf(contact);
                //if (customContacts[contacts.IndexOf(contact)].Number != null)
                    if (!phoneToNameDict.ContainsKey(customContacts[customContactNo].Number.Replace(" ", "").Trim()))
                        phoneToNameDict.Add(customContacts[customContactNo].Number.Replace(" ", "").Trim(), contact.Name);
            }
            PhoneContacts.PhoneToNameDict = phoneToNameDict;
            PhoneContacts.AllCustomContacts = customContacts;
            HomeListView.ItemsSource = customContacts;
            HomeListView.EndRefresh();
        }

        private async void RefreshContacts()
        {
            HomeListView.BeginRefresh();
            List<string> contactsArray = new List<string>();
            
            var contacts = PhoneContacts.AllCustomContacts;
            foreach (Contact contact in contacts)
            {
                
                contactsArray.Add(contact.Number.Replace(" ", "").Trim());
            }
            
            int[] contactCodes = await Services.ChatClient.ConsultAllContactsState(contactsArray.ToArray());
            List<CustomContact> newCustomContacts = contacts;
            for (int i=0; i<contacts.Count; i++)
            {
                newCustomContacts[i].PlayingState = Constants.AppConstants.userStateDict[contactCodes[i]];
            }
            PhoneContacts.AllCustomContacts = newCustomContacts;
            HomeListView.ItemsSource = null;
            
            HomeListView.ItemsSource = newCustomContacts.OrderByDescending(item=>item.PlayingState);
            HomeListView.EndRefresh();

        }

        private async void DuelClicked(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            CustomContact contact = (CustomContact)button.BindingContext;

            /*DO NOT DELETE----------------------------------------------------------
            await DisplayAlert("Test", "You challenged " + contact.Name + " of number " + contact.Number.Replace(" ",""), "OK");
            string rounds = await DisplayActionSheet("Duel options", "Cancel", null, new string[] { "1 round", "3 rounds", "5 rounds", "10 rounds" });
            await DisplayAlert("Selected", "You selected " + rounds , "OK");
            ----------------------------------------------------------------------------*/
            string duelNumber = contact.Number.Contains("+") ? contact.Number : "+" + Globals.PhoneCountryCode.PhoneGlobal + contact.Number;
            Services.ChatClient.SendChallengeUser(1, contact.Number.Replace(" ", "").Trim(), Constants.AppConstants.appUserName);
            //Continuar aqui para crear algun cliente de juego o solo un metodo sino
        }
        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;
            var item = e.Item as CustomContact;
            item.TapCommand.Execute(null);
            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }

        //-----------------------------------------------------------------------------------------


        private async Task LoginToGame()
        {
            await Services.ChatClient.StartConnection(Constants.AppConstants.appUserName);
            Services.ChatClient.AddGameRequestListener(ShowChallengedScreen);
            Services.ChatClient.AddGameCodeListener(ReceiveGameCode);
            RefreshContacts();
            logedIn = true;
            DisconnectButton.IsVisible = true;
        }
        private void ShowChallengedScreen(string challengedBy, int noRounds)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                ShowChallengedScreenMainThread(challengedBy, noRounds);

            });

            /*string message = accept ? "accepted" : "rejected";
            await DisplayAlert("Acceptance", "You just " + message + " the duel", "OK");*/
        }
        private void ReceiveGameCode(int acceptance, int rounds, string against)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                if (acceptance == 1)
                {
                    Navigation.PushAsync(new MainPage(against));
                }
            });
        }
        private async void ShowChallengedScreenMainThread(string challengedBy, int noRounds)
        {
            string challengedName = PhoneContacts.PhoneToNameDict.ContainsKey(challengedBy.Replace(" ","").Trim()) ? PhoneContacts.PhoneToNameDict[challengedBy.Replace(" ","").Trim()] : challengedBy;
            bool accepted = await DisplayAlert("Challenged!", "You were challenged by: " + challengedName, "Accept", "Cancel");
            if (accepted)
            {
                Services.ChatClient.SendAcceptGameRequest(noRounds, challengedBy);
            }
        }
        public void SetupPopupRegistration()
        {
            MainPageLayout.BackgroundColor = Color.LightGray;
            MyListView.IsVisible = false;
            PopupRegister.IsVisible = true;
            PopupRegister.IsEnabled = true;
            CountryNumber.Text = GetPhoneCountryCode();
        }

        async void Handle_MenuItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

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

        private async void DisappearPopup()
        {
            PopupRegister.IsEnabled = false;
            PopupRegister.IsVisible = false;
            WaitingStack.IsVisible = true;
            MyListView.BeginRefresh();
            await LoginToGame();
            ContactsTab.IsVisible = true;
            MyListView.EndRefresh();
            WaitingStack.IsVisible = false;
            MainPageLayout.BackgroundColor = Color.Default;
            MyListView.IsVisible = true;

        }
        private void LoginWithSavedNumber()
        {
            if (Application.Current.Properties.ContainsKey("username"))
            {
                Constants.AppConstants.appUserName = Application.Current.Properties["username"].ToString();
                DisappearPopup();
            }
            else DisplayAlert("Login Fail", "You still dont have a saved number, please enter one", "OK");
        }
        private void SavePhoneNumber(string phoneNumber)
        {
            Application.Current.Properties["username"] = phoneNumber;
        }
        private async void ButtonOk_Clicked(object sender, EventArgs e)
        {
            Constants.AppConstants.appUserName = CountryNumber.Text + PhoneEntry.Text.Trim().Replace(" ", "");
            bool saveNumber = await DisplayAlert("Registration", "Do you want to save number for next time?: " + Constants.AppConstants.appUserName, "Yes", "No");
            if (saveNumber) SavePhoneNumber(Constants.AppConstants.appUserName);
            DisappearPopup();
        }

        private void ButtonLoginWithSaved(object sender, EventArgs e)
        {
            LoginWithSavedNumber();
        }

        private void Refresh_Activated(object sender, EventArgs e)
        {
            if(logedIn)
                RefreshContacts();
        }

        private void ButtonDisconnect_Clicked(object sender, EventArgs e)
        {
            Services.ChatClient.Disconnect(Constants.AppConstants.appUserName);
            DisconnectButton.IsVisible = false;
            ContactsTab.IsVisible = false;
            SetupPopupRegistration();
        }
    }
}